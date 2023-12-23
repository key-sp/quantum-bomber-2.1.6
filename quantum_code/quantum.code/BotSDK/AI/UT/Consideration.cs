using Photon.Deterministic;
using System;
using Quantum.Prototypes;
using Quantum.Collections;

namespace Quantum
{
	[Serializable]
	public struct ResponseCurvePack
	{
		public AssetRefAIFunction ResponseCurveRef;
		[NonSerialized] public ResponseCurve ResponseCurve;
	}

	// ============================================================================================================

	public unsafe partial class Consideration : IConsiderationProvider
	{
		// ========== PUBLIC MEMBERS ==================================================================================

		public string Label;

		public AssetRefAIFunction RankRef;
		public AssetRefAIFunction CommitmentRef;
		public AssetRefConsideration[] NextConsiderationsRefs;
		public AssetRefAIAction[] OnEnterActionsRefs;
		public AssetRefAIAction[] OnUpdateActionsRefs;
		public AssetRefAIAction[] OnExitActionsRefs;

		[NonSerialized] public AIFunction<int> Rank;
		[NonSerialized] public AIFunction<bool> Commitment;
		[NonSerialized] public Consideration[] NextConsiderations;
		[NonSerialized] public AIAction[] OnEnterActions;
		[NonSerialized] public AIAction[] OnUpdateActions;
		[NonSerialized] public AIAction[] OnExitActions;

		public ResponseCurvePack[] ResponseCurvePacks;

		public FP BaseScore;

		public UTMomentumData MomentumData;
		public FP Cooldown;

		public byte Depth;

		// ========== IConsiderationProvider Interface ================================================================
		public AssetRefConsideration GetConsideration(QList<AssetRefConsideration> sourceList, int id)
		{
			return NextConsiderations[id];
		}

		public int Count(QList<AssetRefConsideration> sourceList)
		{
			return NextConsiderations.Length;
		}

		// ========== PUBLIC METHODS ==================================================================================

		public int GetRank(FrameThreadSafe frame, EntityRef entity, ref AIContext aiContext)
		{
			if (Rank == null)
				return 0;

			return Rank.Execute(frame, entity, ref aiContext);
		}

		public FP Score(FrameThreadSafe frame, EntityRef entity, ref AIContext aiContext)
		{
			if (ResponseCurvePacks.Length == 0)
				return 0;

			FP score = 1;
			for (int i = 0; i < ResponseCurvePacks.Length; i++)
			{
        FP scoreWithMultiFactor = ResponseCurvePacks[i].ResponseCurve.Execute(frame, entity, ref aiContext);

        // We re-apply the clamping because the score is scaled by the Multiply Factor
        // So the score can be greater than one. We clamp it as we don't want curves to have un-normalised values
        // as otherwise big and negative values here would semantically result in a "leak" from the curve into the whole consideration
        if (scoreWithMultiFactor > 1) scoreWithMultiFactor = 1;
        else if (scoreWithMultiFactor < 0) scoreWithMultiFactor = 0;

        // We score the Curve based on its input
        score *= scoreWithMultiFactor;

        // If we find a negative veto, the final score would be zero anyways, so we stop here
        if (score == 0)
				{
					break;
				}
			}

      if(score != 0)
      {
        // Apply the compensation formula
        // As many high scoring normalised curves results are multiplied together (like a sequence of 0.9 scores),
        // the general result is degraded. The more curves multiplied, the greater the degrading is
        // This can "break" semantics as more curves would reduce the utility value as a side-effect
        // This compensation formula takes in consideration the amount of curves evaluated in order to compensate a bit
        // for the degrading that happens
        // This expects a normalised value
        FP modificationFactor = FP._1 - (FP._1 / ResponseCurvePacks.Length);
        FP makeUpValue = (FP._1 - score) * modificationFactor;
        score = score + (makeUpValue * score);
      }

      // Then we apply the base score just as a general offset
      score += BaseScore;

      return score;
		}

		public void OnEnter(FrameThreadSafe frame, EntityRef entity, ref AIContext aiContext)
		{
			for (int i = 0; i < OnEnterActions.Length; i++)
			{
				OnEnterActions[i].Update(frame, entity, ref aiContext);
			}
		}

		public void OnExit(FrameThreadSafe frame, EntityRef entity, ref AIContext aiContext)
		{
			for (int i = 0; i < OnExitActions.Length; i++)
			{
				OnExitActions[i].Update(frame, entity, ref aiContext);
			}
		}

		public void OnUpdate(FrameThreadSafe frame, UtilityReasoner* reasoner, EntityRef entity, ref AIContext aiContext)
		{
			for (int i = 0; i < OnUpdateActions.Length; i++)
			{
				OnUpdateActions[i].Update(frame, entity, ref aiContext);
			}

			if (NextConsiderationsRefs != null && NextConsiderationsRefs.Length > 0)
			{
				Consideration chosenConsideration = reasoner->SelectBestConsideration(frame, this, (byte)(Depth + 1),
					reasoner, entity, ref aiContext);

				if (chosenConsideration != default)
				{
					chosenConsideration.OnUpdate(frame, reasoner, entity, ref aiContext);
					BotSDKEditorEvents.UT.InvokeConsiderationChosen(entity, chosenConsideration.Identifier.Guid.Value);
				}
			}
		}

		public override void Loaded(IResourceManager resourceManager, Native.Allocator allocator)
		{
			base.Loaded(resourceManager, allocator);

			Rank = (AIFunction<int>)resourceManager.GetAsset(RankRef.Id);

			if (ResponseCurvePacks != null)
			{
				for (Int32 i = 0; i < ResponseCurvePacks.Length; i++)
				{
					ResponseCurvePacks[i].ResponseCurve = (ResponseCurve)resourceManager.GetAsset(ResponseCurvePacks[i].ResponseCurveRef.Id);
				}
			}

			OnEnterActions = new AIAction[OnEnterActionsRefs == null ? 0 : OnEnterActionsRefs.Length];
			if (OnEnterActionsRefs != null)
			{
				for (Int32 i = 0; i < OnEnterActionsRefs.Length; i++)
				{
					OnEnterActions[i] = (AIAction)resourceManager.GetAsset(OnEnterActionsRefs[i].Id);
				}
			}

			OnUpdateActions = new AIAction[OnUpdateActionsRefs == null ? 0 : OnUpdateActionsRefs.Length];
			if (OnUpdateActionsRefs != null)
			{
				for (Int32 i = 0; i < OnUpdateActionsRefs.Length; i++)
				{
					OnUpdateActions[i] = (AIAction)resourceManager.GetAsset(OnUpdateActionsRefs[i].Id);
				}
			}

			OnExitActions = new AIAction[OnExitActionsRefs == null ? 0 : OnExitActionsRefs.Length];
			if (OnExitActionsRefs != null)
			{
				for (Int32 i = 0; i < OnExitActionsRefs.Length; i++)
				{
					OnExitActions[i] = (AIAction)resourceManager.GetAsset(OnExitActionsRefs[i].Id);
				}
			}

			Commitment = (AIFunction<bool>)resourceManager.GetAsset(CommitmentRef.Id);

			NextConsiderations = new Consideration[NextConsiderationsRefs == null ? 0 : NextConsiderationsRefs.Length];
			if (NextConsiderationsRefs != null)
			{
				for (Int32 i = 0; i < NextConsiderationsRefs.Length; i++)
				{
					NextConsiderations[i] = (Consideration)resourceManager.GetAsset(NextConsiderationsRefs[i].Id);
				}
			}
		}
	}
}
