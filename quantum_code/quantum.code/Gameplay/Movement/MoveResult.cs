using Photon.Deterministic;

namespace Quantum
{
	public readonly struct MoveResult
	{
		public bool IsValid => MaxDistance != FP._0;

		public readonly FPVector2 Direction;
		public readonly FP MaxDistance;
		public readonly FPVector2 LookDirection;
		public readonly FP RotationTimeMultiplier;

		public MoveResult(FPVector2 direction, FP maxDistance)
		{
			Direction = direction;
			MaxDistance = maxDistance;
			LookDirection = direction;
			RotationTimeMultiplier = FP._1;
		}

		public MoveResult(FPVector2 direction, FP maxDistance, FPVector2 lookDirection, FP rotationTimeMultiplier)
		{
			Direction = direction;
			MaxDistance = maxDistance;
			LookDirection = lookDirection;
			RotationTimeMultiplier = rotationTimeMultiplier;
		}
	}
}