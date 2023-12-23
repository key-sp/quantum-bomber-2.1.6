using Photon.Deterministic;

namespace Quantum
{
	public partial struct GridSettings
	{
		// --- Settings Accessors
		// --- Public Methods
		public readonly byte GetWidth() => Width;
		public readonly byte GetHeight() => Height;
		

		// Assumes a 1-cell wide border that is part of the grid.
		public readonly FPVector2 GetGridMin() => FPVector2.One;
		public readonly FPVector2 GetGridMax() => new FPVector2(Width - 2,Height - 2);
		public readonly int GetGridSize() => Width * Height;
		public readonly byte GetCellSize() => CellSize;

		// Currently SpawnPoints follow a hardcoded logic in "SpawnPointSystem"
		public readonly FPVector2[] GetSpawnPoints()
		{
			return new FPVector2[] 
			{

			};
		}
	}
}