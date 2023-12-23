using Photon.Deterministic;
using Quantum.Collections;
using System.Collections;
using System.Collections.Generic;

namespace Quantum
{
	public unsafe partial struct UtilityReasoner : IConsiderationProvider
	{
		// ========== PUBLIC METHODS ==================================================================================

		public AssetRefConsideration GetConsideration(QList<AssetRefConsideration> sourceList, int id)
		{
			return sourceList[id];
		}

		public int Count(QList<AssetRefConsideration> sourceList)
		{
			return sourceList.Count;
		}


		public void Initialize(Frame frame, AssetRefUTRoot utRootRef, EntityRef entity)
		{
			// If we don't receive the UTRoot as parameter, we try to find it on the component itself
			// Useful for pre-seting the UTRoot on a Prototype
			UTRoot utRootInstance;
			if (utRootRef == default)
			{
				utRootInstance = frame.FindAsset<UTRoot>(UTRoot.Id);
			}
			else
			{
				UTRoot = utRootRef;
				utRootInstance = frame.FindAsset<UTRoot>(utRootRef.Id);
			}

			// Initialize the Reasoner's considerations.
			// Can be useful further when creating dynamically added Considerations to the Agent (runtime)
			QList<AssetRefConsideration> considerationsList = frame.AllocateList<AssetRefConsideration>();
			for (int i = 0; i < utRootInstance.ConsiderationsRefs.Length; i++)
			{
				considerationsList.Add(utRootInstance.ConsiderationsRefs[i]);
			}
			Considerations = considerationsList;

			HighRankConsiderations = frame.AllocateList<AssetRefConsideration>();

			CooldownsDict = frame.AllocateDictionary<AssetRefConsideration, FP>();

			QList<AssetRefConsideration> previousExecution = frame.AllocateList<AssetRefConsideration>();
			for (int i = 0; i < 6; i++)
			{
				previousExecution.Add(default);
			}
			PreviousExecution = previousExecution;

			MomentumList = frame.AllocateList<UTMomentumPack>();

			if (entity != default)
			{
        BotSDKEditorEvents.UT.InvokeSetupDebugger(entity, utRootInstance.Path);
			}
		}

		public void Free(Frame frame)
		{
			UTRoot = default;
			frame.FreeList<AssetRefConsideration>(Considerations);
			frame.FreeDictionary<AssetRefConsideration, FP>(CooldownsDict);
			frame.FreeList<AssetRefConsideration>(PreviousExecution);
			frame.FreeList<UTMomentumPack>(MomentumList);
		}

		public void Update(FrameThreadSafe frame, UtilityReasoner* reasoner, EntityRef entity, ref AIContext aiContext)
		{
			Consideration chosenConsideration = SelectBestConsideration(frame, this, 1, reasoner, entity, ref aiContext);

			if (chosenConsideration != default)
			{
				chosenConsideration.OnUpdate(frame, reasoner, entity, ref aiContext);
        BotSDKEditorEvents.UT.InvokeConsiderationChosen(entity, chosenConsideration.Identifier.Guid.Value);
			}

			TickMomentum(frame, entity, ref aiContext);
		}

		public Consideration SelectBestConsideration<T>(FrameThreadSafe frame, T considerationProvider,
			byte depth, UtilityReasoner* reasoner, EntityRef entity, ref AIContext aiContext) where T : IConsiderationProvider
		{
			QList<UTMomentumPack> momentumList = frame.ResolveList(MomentumList);

			// We get the Rank of every Consideration Set
			// This "filters" the Considerations with higher absolute utility
			QList<AssetRefConsideration> highRankConsiderationsRefs = frame.ResolveList(HighRankConsiderations);
			if (highRankConsiderationsRefs.Count > 0)
			{
				highRankConsiderationsRefs.Clear();
			}

			int highestRank = -1;
			QDictionary<AssetRefConsideration, FP> cooldowns = frame.ResolveDictionary(CooldownsDict);

			QList<AssetRefConsideration> solvedConsiderationsList = frame.ResolveList(Considerations);

			for (int i = 0; i < considerationProvider.Count(solvedConsiderationsList); i++)
			{
				if(considerationProvider.GetConsideration(solvedConsiderationsList, i) == null)
				{
					break;
				}

				AssetRefConsideration considerationRef = considerationProvider.GetConsideration(solvedConsiderationsList, i);
				Consideration consideration = frame.FindAsset<Consideration>(considerationRef.Id);

				// Force low Rank for Considerations in Cooldown
				if (cooldowns.Count > 0 && cooldowns.ContainsKey(considerationRef) == true)
				{
					cooldowns[considerationRef] -= frame.DeltaTime;
					if (cooldowns[considerationRef] <= 0)
					{
						cooldowns.Remove(considerationRef);
					}
					{
						continue;
					}
				}

				// If the Consideration has Momentum, then it's Rank should is defined by it
				// Otherwise, we calculate the Rank dynamically
				int rank;
				if (ContainsMomentum(momentumList, consideration, out var momentum) == true)
				{
					rank = momentum.Value;
				}
				else
				{
					rank = consideration.GetRank(frame, entity, ref aiContext);
				}

				if (rank > highestRank)
				{
					highestRank = rank;
					highRankConsiderationsRefs.Clear();
					highRankConsiderationsRefs.Add(considerationRef);

				}
				else if (highestRank == rank)
				{
					highRankConsiderationsRefs.Add(considerationRef);
				}
			}

			// Based on the higher rank, we check which Considerations sets have greater utility
			// Then we choose that set this frame
			Consideration chosenConsideration = default;
			FP highestScore = FP.UseableMin;
			for (int i = 0; i < highRankConsiderationsRefs.Count; i++)
			{
				if (highRankConsiderationsRefs[i] == default)
					continue;

				Consideration consideration = frame.FindAsset<Consideration>(highRankConsiderationsRefs[i].Id);
				FP score = consideration.Score(frame, entity, ref aiContext);
				if (highestScore < score)
				{
					highestScore = score;
					chosenConsideration = consideration;
				}
			}

			if (chosenConsideration != default)
			{
				// If the chosen Consideration and it is not already under Momentum,
				// we add add it there, replacing the previous Momentum (if any)
				if (chosenConsideration.MomentumData.Value > 0 && ContainsMomentum(momentumList, chosenConsideration, out var momentum) == false)
				{
					InsertMomentum(frame, momentumList, chosenConsideration);
				}

				// If the chosen Consideration has cooldown and it is not yet on the cooldowns dictionary,
				// we add it there
				if (chosenConsideration.Cooldown > 0 && cooldowns.ContainsKey(chosenConsideration) == false)
				{
					cooldowns.Add(chosenConsideration, chosenConsideration.Cooldown);
				}

				// Add the chosen set to the choices history
				OnConsiderationChosen(frame, reasoner, chosenConsideration, entity, ref aiContext);
			}
			else
			{
				OnNoConsiderationChosen(frame, reasoner, depth, entity, ref aiContext);
			}

			// We return the chosen set so it can be executed
			return chosenConsideration;
		}

		// ========== PRIVATE METHODS =================================================================================

		#region Momentum
		private bool ContainsMomentum(QList<UTMomentumPack> momentumList, Consideration consideration, out UTMomentumData momentum)
		{
			for (int i = 0; i < momentumList.Count; i++)
			{
				if (momentumList[i].ConsiderationRef == consideration)
				{
					momentum = momentumList[i].MomentumData;
					return true;
				}
			}

			momentum = default;
			return false;
		}

		private void InsertMomentum(FrameThreadSafe frame, QList<UTMomentumPack> momentumList, AssetRefConsideration considerationRef)
		{
			Consideration newConsideration = frame.FindAsset<Consideration>(considerationRef.Id);

			// First, we check if this should be a replacement, which happens if:
			// . The momentum list already have that same Depth added
			// . Or when it have a higher Depth added
			bool wasReplacedment = false;
			for (int i = 0; i < momentumList.Count; i++)
			{
				Consideration currentConsideration = frame.FindAsset<Consideration>(momentumList[i].ConsiderationRef.Id);
				if (currentConsideration.Depth == newConsideration.Depth || currentConsideration.Depth > newConsideration.Depth)
				{
					momentumList.GetPointer(i)->ConsiderationRef = considerationRef;
					momentumList.GetPointer(i)->MomentumData = frame.FindAsset<Consideration>(considerationRef.Id).MomentumData;

					// We clear the rightmost momentum entries
					if (i < momentumList.Count - 1)
					{
						for (int k = i + 1; k < momentumList.Count; k++)
						{
							momentumList.RemoveAt(k);
						}
					}

					wasReplacedment = true;
					break;
				}
			}

			// If there was no replacement, we simply add it to the end of the list as this
			// consideration probably has higher Depth than the others currently on the list
			// which can also mean that the list was empty
			if (wasReplacedment == false)
			{
				UTMomentumPack newMomentum = new UTMomentumPack()
				{
					ConsiderationRef = considerationRef,
					MomentumData = frame.FindAsset<Consideration>(considerationRef.Id).MomentumData,
				};
				momentumList.Add(newMomentum);
			}
		}

		private void TickMomentum(FrameThreadSafe frame, EntityRef entity, ref AIContext aiContext)
		{
			QList<UTMomentumPack> momentumList = frame.ResolveList(MomentumList);

			// We decrease the timer and check if it is time already to decay all of the current Momentums
			TimeToTick -= frame.DeltaTime;
			bool decay = false;
			if (TimeToTick <= 0)
			{
				decay = true;
				TimeToTick = 1;
			}

			for (int i = 0; i < momentumList.Count; i++)
			{
				UTMomentumPack* momentum = momentumList.GetPointer(i);

				// If we currently have a commitment, we check if it is done already
				// If it is done, that Consideration's Rank shall be re-calculated
				// If it is not done, then the Consideration's Rank will be kept due to the commitment
				// unless some other Consideration has greater Rank and replaces the current commitment
				Consideration momentumConsideration = frame.FindAsset<Consideration>(momentum->ConsiderationRef.Id);

				if (momentum->MomentumData.Value > 0 && momentumConsideration.MomentumData.DecayAmount > 0)
				{
					if (decay)
					{
						momentum->MomentumData.Value -= momentumConsideration.MomentumData.DecayAmount;
					}
				}


				bool isDone = false;
				if (momentumConsideration.Commitment != default)
				{
					isDone = momentumConsideration.Commitment.Execute(frame, entity, ref aiContext);
				}
				if (isDone == true || momentum->MomentumData.Value <= 0)
				{
					momentum->MomentumData.Value = 0;
					momentumList.RemoveAt(i);
				}
			}
		}
		#endregion

		#region ConsiderationsChoiceReactions
		private static void OnConsiderationChosen(FrameThreadSafe frame, UtilityReasoner* reasoner, 
			AssetRefConsideration chosenConsiderationRef, EntityRef entity, ref AIContext aiContext)
		{
			Consideration chosenConsideration = frame.FindAsset<Consideration>(chosenConsiderationRef.Id);

			QList<AssetRefConsideration> previousExecution = frame.ResolveList(reasoner->PreviousExecution);

			if (previousExecution[chosenConsideration.Depth - 1] != chosenConsideration)
			{
				// Exit the one that we're replacing
				var replacedSet = frame.FindAsset<Consideration>(previousExecution[chosenConsideration.Depth - 1].Id);
				if (replacedSet != default)
				{
					replacedSet.OnExit(frame, entity, ref aiContext);
				}

				// Exit the consecutive ones
				for (int i = chosenConsideration.Depth; i < previousExecution.Count; i++)
				{
					var cs = frame.FindAsset<Consideration>(previousExecution[i].Id);
					if (cs == default)
						break;

					cs.OnExit(frame, entity, ref aiContext);
					previousExecution[i] = default;
				}

				// Insert and Enter on the new chosen consideration
				previousExecution[chosenConsideration.Depth - 1] = chosenConsideration;
				chosenConsideration.OnEnter(frame, entity, ref aiContext);
			}
		}

		private static void OnNoConsiderationChosen(FrameThreadSafe frame, UtilityReasoner* reasoner, byte depth, 
			EntityRef entity, ref AIContext aiContext)
		{
			QList<AssetRefConsideration> previousExecution = frame.ResolveList(reasoner->PreviousExecution);

			for (int i = depth - 1; i < previousExecution.Count; i++)
			{
				var cs = frame.FindAsset<Consideration>(previousExecution[i].Id);
				if (cs == default)
					break;

				cs.OnExit(frame, entity, ref aiContext);
				previousExecution[i] = default;
			}
		}
		#endregion
	}
}
