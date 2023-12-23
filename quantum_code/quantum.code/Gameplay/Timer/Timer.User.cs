using Photon.Deterministic;

namespace Quantum
{
	public partial struct Timer
	{
		public bool IsRunning => Tick > 0; 
		
		public bool HasExpired(Frame f)
		{
			return f.Number > Tick;
		}

		public void SetFromTime(Frame f, FP seconds)
		{
			var ticks = FPMath.RoundToInt(seconds / f.DeltaTime);
			SetFromTicks(f, ticks);
		}

		public void SetFromTicks(Frame f, int ticks)
		{
			if (ticks < 0) return;
			ticks = ticks < 1 ? 1 : ticks;
			Tick = f.Number + ticks;
		}

		public void Reset()
		{
			Tick = -1;
		}

		public void ExpiresImmediately(Frame f)
		{
			Tick = f.Number - 1;
		}

		public FP GetRemainingTime(Frame f)
		{
			var remainingTicks = Tick - f.Number;
			var remainingTime = remainingTicks * f.DeltaTime;
			return remainingTime > FP._0 ? remainingTime : FP._0;
		}
	}
}