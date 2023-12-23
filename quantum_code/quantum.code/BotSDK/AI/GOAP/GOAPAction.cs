using Photon.Deterministic;
using System.Collections.Generic;

namespace Quantum
{
	public abstract unsafe partial class GOAPAction
	{
		public enum EResult
		{
			Continue,
			IsDone,
			IsFailed,
		}

		// PUBLIC MEMBERS

		public string    Label;

		[BotSDKHidden]
		public GOAPState Conditions;
		[BotSDKHidden]
		public GOAPState Effects;

		public bool      Interruptible;

		public abstract bool UsePlanStateValidation { get; }

		// PUBLIC METHODS

		public virtual bool ValidateAction(Frame frame, GOAPEntityContext context, ref AIContext aiContext, GOAPState startState, out FP cost)
		{
			cost = 1;
			return true;
		}

		public virtual void ValidatePlanState(Frame frame, GOAPEntityContext context, ref AIContext aiContext, GOAPState stateToValidate, GOAPState nextState, FP costToNextState, List<StateBackValidation> validatedStates)
		{
		}

		public virtual void Activate(Frame frame, GOAPEntityContext context, ref AIContext aiContext)
		{
		}

		public virtual EResult Update(Frame frame, GOAPEntityContext context, ref AIContext aiContext)
		{
			return EResult.Continue;
		}

		public virtual void Deactivate(Frame frame, GOAPEntityContext context, ref AIContext aiContext)
		{
		}
	}
}