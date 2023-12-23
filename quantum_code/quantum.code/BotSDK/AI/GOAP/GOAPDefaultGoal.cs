using System;
using Photon.Deterministic;

namespace Quantum
{
	[Serializable]
	public unsafe partial class GOAPDefaultGoal : GOAPGoal
	{
		// PUBLIC MEMBERS

		public AIParamBool          Validation = true;
		public AIParamFP            Relevancy = FP._1;
		public AIParamFP            DisableTime;
		public AssetRefAIAction[]   OnInitPlanningLinks;
		public AssetRefAIAction[]   OnActivateLinks;
		public AssetRefAIAction[]   OnDeactivateLinks;
		public AIParamBool          IsFinished;

		[NonSerialized]
		public AIAction[] OnInitPlanning;
		[NonSerialized]
		public AIAction[] OnActivate;
		[NonSerialized]
		public AIAction[] OnDeactivate;

		// PUBLIC METHODS

		public override FP GetRelevancy(Frame frame, GOAPEntityContext context, ref AIContext aiContext)
		{
			if (Validation.Resolve(frame, context.Entity, context.Blackboard, context.Config, ref aiContext) == false)
				return 0;

			return Relevancy.Resolve(frame, context.Entity, context.Blackboard, context.Config, ref aiContext);
		}

		public override void InitPlanning(Frame frame, GOAPEntityContext context, ref AIContext aiContext, ref GOAPState startState, ref GOAPState targetState)
		{
			base.InitPlanning(frame, context, ref aiContext, ref startState, ref targetState);

			ExecuteActions(frame, context.Entity, OnInitPlanning, ref aiContext);
		}

		public override void Activate(Frame frame, GOAPEntityContext context, ref AIContext aiContext)
		{
			base.Activate(frame, context, ref aiContext);

			ExecuteActions(frame, context.Entity, OnActivate, ref aiContext);
		}

		public override void Deactivate(Frame frame, GOAPEntityContext context, ref AIContext aiContext)
		{
			ExecuteActions(frame, context.Entity, OnDeactivate, ref aiContext);

			base.Deactivate(frame, context, ref aiContext);
		}

		public override bool HasFinished(Frame frame, GOAPEntityContext context, ref AIContext aiContext)
		{
			if (base.HasFinished(frame, context, ref aiContext) == true)
				return true;

			return IsFinished.Resolve(frame, context.Entity, context.Blackboard, context.Config, ref aiContext);
		}

		public override FP GetDisableTime(Frame frame, GOAPEntityContext context, ref AIContext aiContext)
		{
			return DisableTime.Resolve(frame, context.Entity, context.Blackboard, context.Config, ref aiContext);
		}

		// AssetObject INTERFACE

		public override void Loaded(IResourceManager resourceManager, Native.Allocator allocator)
		{
			base.Loaded(resourceManager, allocator);

			OnInitPlanning = new AIAction[OnInitPlanningLinks == null ? 0 : OnInitPlanningLinks.Length];
			for (int i = 0; i < OnInitPlanning.Length; i++)
			{
				OnInitPlanning[i] = (AIAction)resourceManager.GetAsset(OnInitPlanningLinks[i].Id);
			}

			OnActivate = new AIAction[OnActivateLinks == null ? 0 : OnActivateLinks.Length];
			for (int i = 0; i < OnActivate.Length; i++)
			{
				OnActivate[i] = (AIAction)resourceManager.GetAsset(OnActivateLinks[i].Id);
			}

			OnDeactivate = new AIAction[OnDeactivateLinks == null ? 0 : OnDeactivateLinks.Length];
			for (int i = 0; i < OnDeactivate.Length; i++)
			{
				OnDeactivate[i] = (AIAction)resourceManager.GetAsset(OnDeactivateLinks[i].Id);
			}
		}

		// PRIVATE METHODS

		private static void ExecuteActions(Frame frame, EntityRef entity, AIAction[] actions, ref AIContext aiContext)
		{
			for (int i = 0; i < actions.Length; i++)
			{
				var action = actions[i];

				action.Update(frame, entity, ref aiContext);

				int nextAction = action.NextAction(frame, entity);
				if (nextAction > i)
				{
					i = nextAction;
				}
			}
		}
	}
}