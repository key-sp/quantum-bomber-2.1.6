using Photon.Deterministic;

namespace Quantum
{
	public partial struct AbilityPlaceBomb
	{
		public bool CanPlaceBomb => CurrentlyActiveBombs < BombsAmount;
		public void BombExploded() => CurrentlyActiveBombs--;
		public void BombPlaced() => CurrentlyActiveBombs++;

		public void ModifyBombAmount(Frame f, FP amount, bool maxOut = false)
		{
			var config = f.FindAsset<AbilityPlaceBombConfig>(Config.Id);
			
			if (maxOut) {
				BombsAmount = config.BombsAmountMax;
			}
			else {
				BombsAmount = (byte) (BombsAmount + amount.RoundToIntSign());
			}

			BombsAmount = (byte) FPMath.Clamp(BombsAmount, 1, config.BombsAmountMax);
		}

		public void ModifyBombPower(Frame f, FP amount,bool maxOut = false)
		{
			var config = f.FindAsset<AbilityPlaceBombConfig>(Config.Id);

			if (maxOut) {
				BombReach = config.BombsReachMax;
			}
			else {
				BombReach = (byte) (BombReach + amount.RoundToIntSign());
			}

			BombReach = (byte) FPMath.Clamp(BombReach, 1, config.BombsReachMax);
		}
	}
}