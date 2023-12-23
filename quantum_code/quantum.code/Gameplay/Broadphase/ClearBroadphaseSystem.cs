using Photon.Deterministic;

namespace Quantum
{
	public unsafe class ClearBroadphaseSystem : SystemMainThread
	{
		public override void Update(Frame f)
		{
			var bombFilter = f.Filter<Transform2D, Bomb, ClearGrid>();
			while (bombFilter.NextUnsafe(out var entity, out var transform, out _, out _)) {
				ClearCellWithBomb(f, transform->Position);
				f.Destroy(entity);
			}
			
			var explosionFilter = f.Filter<Transform2D, Explosion, ClearGrid>();
			while (explosionFilter.NextUnsafe(out var entity, out var transform, out var explosion, out _)) {
				ClearCellWithExplosion(f, transform->Position, explosion);
				f.Destroy(entity);
			} 
			
			var blockDestroyableFilter = f.Filter<Transform2D, BlockDestroyable, ClearGrid>();
			while (blockDestroyableFilter.NextUnsafe(out var entity, out var transform, out _, out _)) {
				ClearCellWithBlockDestroyable(f, transform->Position);
				f.Destroy(entity);
			}

			var powerUpFilter = f.Filter<Transform2D, PowerUp, ClearGrid>();
			while (powerUpFilter.NextUnsafe(out var entity, out var transform, out _, out _)) {
				ClearCellWithPowerUp(f, transform->Position);
				f.Destroy(entity);
			}
		}

		private void ClearCellWithBomb(Frame f, FPVector2 position)
		{
			var cell = f.Grid.GetCellPtr(position);
			cell->Set(CellType.Empty);
		}

		private void ClearCellWithExplosion(Frame f, FPVector2 explosionOrigin, Explosion* explosion)
		{
			var position = default(FPVector2);

			var cell = f.Grid.GetCellPtr(explosionOrigin);
			cell->Remove(CellType.Explosion);
			
			for (var i = 0; i < Constants.CARDINAL_DIRECTION_COUNT; i++)
			{
				var direction = Direction_ext.ConvertIntToDirection(i);
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
					
					cell = f.Grid.GetCellPtr(position);
					cell->Remove(CellType.Explosion);
				}
			}
		}

		private void ClearCellWithBlockDestroyable(Frame f, FPVector2 position)
		{
			var cell = f.Grid.GetCellPtr(position);
			cell->Add(CellType.Empty);
			cell->Remove(CellType.BlockDestroyable);
		}
		
		private void ClearCellWithPowerUp(Frame f, FPVector2 position)
		{
			var cell = f.Grid.GetCellPtr(position);
			cell->Remove(CellType.PowerUp);
		}
	}
}