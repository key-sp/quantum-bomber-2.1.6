using System.Collections.Generic;
using Photon.Deterministic;

namespace Quantum
{
	public unsafe class SpawnPointSystem : SystemSignalsOnly
	{
		private static readonly FPVector2[] DIRECTION_VECTORS = {
			FPVector2.Up, FPVector2.Right, FPVector2.Down, FPVector2.Left
		};
		
		public override void OnInit(Frame f)
		{
			var playerCount = (byte) f.PlayerCount;
			
			if (playerCount > 9) {
				Log.Warn("The sample currently only creates 9 Spawn Points. " +
				         "In the current version, a player count > 9 will result in players sharing spawn points.");
			}
			
			f.Global->SpawnPoints = f.AllocateList<FPVector2>(playerCount);
			var spawnPoints = f.ResolveList(f.Global->SpawnPoints);
			spawnPoints.Add(FPVector2.One);
			
			CreateDefaultSpawnPoints(f, playerCount);
			AdjustSpawnPointLocation(f);
			ClearSpawnPointSurroundings(f); 
		}

		private void CreateDefaultSpawnPoints(Frame f, byte playerCount)
		{
			// This method only accounts for up to 9 players!
			
			var spawnPoints = f.ResolveList(f.Global->SpawnPoints);
			
			// Remove the level borders from the count
			var usableWidth = f.Grid.GetGridWidth() - 2;
			var usableHeight = f.Grid.GetGridHeight() - 2;
			
			var xMiddle = FindMidPoint(f.Grid.GetGridWidth());
			var yMiddle = FindMidPoint(f.Grid.GetGridHeight());
			
			FPVector2[] defaultSpawnPoints = {
				FPVector2.One,
				new FPVector2(usableWidth, 1),
				new FPVector2(1, usableHeight),
				new FPVector2(usableWidth, usableHeight),
			};

			foreach (var spawnPoint in defaultSpawnPoints) {
				spawnPoints.Add(spawnPoint);
			}

			if (playerCount > 4)
			{
				var centralSpawnPoint = new FPVector2(xMiddle, yMiddle);
				spawnPoints.Add(centralSpawnPoint);
			}
			
			if (playerCount > 5)
			{
				FPVector2[] midsectionSpawnPoints = {
					new FPVector2(xMiddle, 1),
					new FPVector2(xMiddle, usableHeight),
					new FPVector2(1, yMiddle),
					new FPVector2(usableWidth, yMiddle),
				};
				
				foreach (var spawnPoint in midsectionSpawnPoints) {
					spawnPoints.Add(spawnPoint);
				}			
			}
		}
		
		private void AdjustSpawnPointLocation(Frame f)
		{
			var spawnPoints = f.ResolveList(f.Global->SpawnPoints);
			var position = default(FPVector2);
			
			for(var i = 0; i < spawnPoints.Count; i++)
			{
				// Clear Spawn Position
				position = spawnPoints[i];
				var cell = f.Grid.GetCellPtr(position);
				
				if (cell->IsFix == false) continue;
				
				var adjustOnX = f.FlipCoin();
				var positiveOffset = f.FlipCoin();
				if (adjustOnX) {
					position.X += positiveOffset ? 1 : -1;
				} else {
					position.Y += positiveOffset ? 1 : -1;
				}

				spawnPoints[i] = position;
			}
		}
		
		private void ClearSpawnPointSurroundings(Frame f)
		{
			var spawnPoints = f.ResolveList(f.Global->SpawnPoints);
			var position = default(FPVector2);
			
			foreach (var spawnPoint in spawnPoints)
			{
				// Clear Spawn Position
				position = spawnPoint;
				var cell = f.Grid.GetCellPtr(position);
				cell->Set(CellType.Empty);
        
				// Clear Adjacent Destroyable Blocks
				foreach (var direction in DIRECTION_VECTORS)
				{
					position = spawnPoint + direction;
					cell = f.Grid.GetCellPtr(position);

					// Skip fixed cells
					if (cell->IsFix) continue;
          
					cell->Set(CellType.Empty);
				}
			}
		}

		private int FindMidPoint(int value)
		{
			Assert.Always(value > 2);
			value = FPMath.FloorToInt((FP)value / (FP)2);
			if (value % 2 == 0) value += 1;
			return value;
		}
	}
}