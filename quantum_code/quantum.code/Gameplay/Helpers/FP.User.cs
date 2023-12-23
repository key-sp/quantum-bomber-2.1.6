using Photon.Deterministic;

namespace Quantum
{
	public static class FP_Extensions
	{
		/// <summary>
		/// Used to ensure the FP value is a whole value. If it is not, it rounds it up or down to the nearest Int while keeping the Sign.
		/// </summary>
		/// <param name="value">FP</param>
		/// <returns></returns>
		public static FP RoundToIntSign(this FP value)
		{
			var sign = FPMath.Sign(value);
			var nearestInt = FPMath.RoundToInt(FPMath.Abs(value)); 
			return sign * nearestInt;	
		}
	}
}