using Photon.Deterministic;

namespace Quantum
{
	public static class FPVector2_Extensions
	{
		public static FPVector2 CeilToInt(this FPVector2 vector)
		{
			return new FPVector2(FPMath.CeilToInt(vector.X), FPMath.CeilToInt(vector.Y));
		}
	
		public static FPVector2 FloorToInt(this FPVector2 vector)
		{
			return new FPVector2(FPMath.FloorToInt(vector.X), FPMath.FloorToInt(vector.Y));
		}
	
		public static FPVector2 RoundToInt(this FPVector2 vector, Axis axis = Axis.Both)
		{
			var roundedVector = vector;

			switch (axis)
			{
				case Axis.X:
					roundedVector.X = FPMath.RoundToInt(vector.X);
					break;
				case Axis.Y:
					roundedVector.Y = FPMath.RoundToInt(vector.Y);
					break;
				case Axis.Both:
					roundedVector.X = FPMath.RoundToInt(vector.X);
					roundedVector.Y = FPMath.RoundToInt(vector.Y);
					break;
				default:
					Log.Warn($"Axis {axis} is INVALID for position {vector}");
					break;
			}

			return roundedVector;
		}
	}
}