﻿using Photon.Deterministic;
using Quantum.Collections;
using System;
using System.Runtime.CompilerServices;

namespace Quantum
{
	public static unsafe partial class BTManager
	{
		/// <summary>
		/// Call this once, to initialize the BTAgent.
		/// This method internally looks for a Blackboard Component on the entity
		/// and passes it down the pipeline.
		/// </summary>
		/// <param name="frame"></param>
		/// <param name="entity"></param>
		/// <param name="root"></param>
		public static void Init(Frame frame, EntityRef entity, BTRoot root, int compoundId = -1)
		{
			AIContext aiContext = new AIContext();
			Init(frame, entity, root, ref aiContext, compoundId);
		}

		/// <summary>
		/// Call this once, to initialize the BTAgent.
		/// This method internally looks for a Blackboard Component on the entity
		/// and passes it down the pipeline.
		/// </summary>
		/// <param name="frame"></param>
		/// <param name="entity"></param>
		/// <param name="root"></param>
		public static void Init(Frame frame, EntityRef entity, BTRoot root, ref AIContext aiContext, int compoundId = -1)
		{
			if (compoundId == -1)
			{
				if (frame.Unsafe.TryGetPointer(entity, out BTAgent* agent))
				{
					agent->Initialize(frame, entity, agent, root, true, false);
				}
				else
				{
					Log.Error("[Bot SDK] Tried to initialize an entity which has no BTAgent component");
				}
			}
			else if (frame.Unsafe.TryGetPointer(entity, out CompoundBTAgent* compoundAgent))
			{
				if (compoundAgent->BTAgents.Ptr == default)
				{
					compoundAgent->BTAgents = frame.AllocateList<BTAgent>();
				}

				var agents = frame.ResolveList(compoundAgent->BTAgents);
				while (agents.Count < compoundId + 1)
				{
					agents.Add(new BTAgent());
				}

				var agentPtr = agents.GetPointer(compoundId);
				agentPtr->Initialize(frame, entity, agentPtr, root, true, true);
			}
		}

		/// <summary>
		/// Made for internal use only.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClearBTParams(BTParams btParams)
		{
			btParams.Reset();
		}

		/// <summary>
		/// Call this method every frame to update your BT Agent.
		/// You can optionally pass a Blackboard Component to it, if your Agent uses it
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Update(Frame frame, EntityRef entity, AIBlackboardComponent* blackboard = null, int compoundId = -1)
		{
			AIContext aiContext = new AIContext();
			Update(frame, entity, ref aiContext, blackboard, compoundId);
		}

		/// <summary>
		/// Call this method every frame to update your BT Agent.
		/// You can optionally pass a Blackboard Component to it, if your Agent uses it
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Update(Frame frame, EntityRef entity, ref AIContext aiContext, AIBlackboardComponent* blackboard = null, int compoundId = -1)
		{
			ThreadSafe.Update((FrameThreadSafe)frame, entity, ref aiContext, blackboard, compoundId);
		}

		/// <summary>
		/// CAUTION: Use this overload with care.<br/>
		/// It allows the definition of custom parameters which are passed through the entire BT pipeline, for easy access.<br/>
		/// The user parameters struct needs to be created from scratch every time BEFORE calling the BT Update method.<br/>
		/// Make sure to also implement BTParamsUser.ClearUser(frame).
		/// </summary>
		/// <param name="userParams">Used to define custom user data. It needs to be created from scratch every time before calling this method.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Update(Frame frame, EntityRef entity, ref BTParamsUser userParams, AIBlackboardComponent* blackboard = null, int compoundId = -1)
		{
			AIContext aiContext = new AIContext();
			Update(frame, entity, ref userParams, ref aiContext, blackboard, compoundId);
		}
		/// <summary>
		/// CAUTION: Use this overload with care.<br/>
		/// It allows the definition of custom parameters which are passed through the entire BT pipeline, for easy access.<br/>
		/// The user parameters struct needs to be created from scratch every time BEFORE calling the BT Update method.<br/>
		/// Make sure to also implement BTParamsUser.ClearUser(frame).
		/// </summary>
		/// <param name="userParams">Used to define custom user data. It needs to be created from scratch every time before calling this method.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Update(Frame frame, EntityRef entity, ref BTParamsUser userParams, ref AIContext aiContext, AIBlackboardComponent* blackboard = null, int compoundId = -1)
		{
			ThreadSafe.Update((FrameThreadSafe)frame, entity, ref userParams, ref aiContext, blackboard, compoundId);
		}
	}
}
