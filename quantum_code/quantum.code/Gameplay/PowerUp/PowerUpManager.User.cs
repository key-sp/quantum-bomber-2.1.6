using Photon.Deterministic;

namespace Quantum
{
	public partial struct PowerUpManager
	{
		public void AddNewSpawnPosition(Frame f, FPVector2 spawnPosition)
		{
			var spawnPositions = f.ResolveList(SpawnPositions);
			spawnPositions.Add(spawnPosition);
		}
	}
}