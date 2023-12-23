using Photon.Deterministic;

namespace Quantum
{
	public partial class MovementConfig
	{
		// Not implemented yet
		// public FPVector2 Offset;
		// public FP MaxPenetration;
		// public FP PenetrationCorrection;
		// public FP Extent;
		
		public FP Acceleration;
		public FP Braking;
		public FP DefaultSpeed; // 5
		public FP MinSpeed;
		public FP MaxSpeed; // 10

		public FP DefaultSpeedRotationSpeed; // 0.2
		public FP MaxSpeedRotationTime; // 0.09999

		// Toggles not implemented yet. Currently both are allowed by default.
		// public QBoolean AllowCornerSlide;
		// public QBoolean AllowDiagonalMovement;
	}
}