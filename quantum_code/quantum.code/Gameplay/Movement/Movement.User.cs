using Photon.Deterministic;

namespace Quantum
{
	public partial struct Movement
	{
		public void ModifySpeed(Frame f, FP speedModifier, bool maxOut = false)
		{
			var movementConfig = f.FindAsset<MovementConfig>(Config.Id);
			
			if (maxOut) {
				MaxSpeed = movementConfig.MaxSpeed;
			}
			else {
				MaxSpeed += speedModifier;
				MaxSpeed = FPMath.Clamp(MaxSpeed, movementConfig.MinSpeed, movementConfig.MaxSpeed);
			}
		}
	}
}