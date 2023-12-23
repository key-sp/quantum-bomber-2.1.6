using Photon.Deterministic;

namespace Quantum
{
	public unsafe class SetBroadphaseSystem : SystemMainThread
	{
		public override void Update(Frame f)
		{
			var bombFilter = f.Filter<Transform2D, Bomb>();
			while (bombFilter.Next(out _, out var transform, out _)) {
				SetCellWithBomb(f, transform.Position);
			}

			var explosionFilter = f.Filter<Transform2D, Explosion>();
			while (explosionFilter.NextUnsafe(out _, out var transform, out var explosion)) {
				SetCellWithExplosion(f, transform->Position, explosion);
			}

			// Unnecessary at the moment.
			// Will become useful one AI + Dijkstra is implemented.
			var powerUpFilter = f.Filter<Transform2D, PowerUp>();
			while (powerUpFilter.NextUnsafe(out _, out var transform, out var powerUp)) {
				SetCellWithPowerUp(f, transform->Position);
			}

			// DebugDrawSpawnPoints(f);
		}

		private void DebugDrawSpawnPoints(Frame f)
		{
			var spawnPoints = f.ResolveList(f.Global->SpawnPoints);
			foreach (var spawnPoint in spawnPoints)
			{
				Draw.Circle(spawnPoint, FP._0_25, ColorRGBA.Red);
			}
		}
		
		private void SetCellWithBomb(Frame f, FPVector2 position)
		{
			var cell = f.Grid.GetCellPtr(position);
			cell->Set(CellType.Bomb);
		}

		private void SetCellWithExplosion(Frame f, FPVector2 explosionOrigin, Explosion* explosion)
		{
			var position = default(FPVector2);

			var cellOrigin = f.Grid.GetCellPtr(explosionOrigin);
			cellOrigin->Add(CellType.Explosion);
			
			// Draw.Circle(explosionOrigin, FP._0_10, ColorRGBA.Blue);
			
			for (var i = 0; i < Constants.CARDINAL_DIRECTION_COUNT; i++)
			{
				var direction = Direction_ext.ConvertIntToDirection(i);
				
				// Starts at 1 as index 0 is the center of the explosion for all sides
				for (var j = 1; j <= explosion->ReachDirection[i]; j++)
				{
					position = explosionOrigin;
					switch (direction)
					{
						case Direction.Right:
							position += FPVector2.Right * j;
							break;
						case Direction.Left:
							position += FPVector2.Left * j;
							break;
						case Direction.Up:
							position += FPVector2.Up * j;
							break;
						case Direction.Down:
							position += FPVector2.Down * j;
							break;
					}

					var cellNeighbor = f.Grid.GetCellPtr(position);
					
					if (cellNeighbor->IsFix) continue;
					
					cellNeighbor->Add(CellType.Explosion);
					
					// Draw.Circle(position, FP._0_10, ColorRGBA.Blue);
				}
			}
		}
		
		private void SetCellWithPowerUp(Frame f, FPVector2 position)
		{
			var cell = f.Grid.GetCellPtr(position);
			cell->Add(CellType.PowerUp);
		}
	}
	
}