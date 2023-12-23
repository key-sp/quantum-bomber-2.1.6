using Photon.Deterministic;

namespace Quantum
{
	public unsafe class ExplosionSystem : SystemMainThreadFilter<ExplosionSystem.ExplosionFilter>
	{
		public struct ExplosionFilter
		{
			public EntityRef Entity;
			public Explosion* Explosion;
			public Timer* Timer;
		}

		public override void Update(Frame f, ref ExplosionFilter filter)
		{
			if (filter.Timer->IsRunning == false) {
				filter.Timer->SetFromTime(f, filter.Explosion->CellSpreadTime);
			}
			
			if (filter.Timer->HasExpired(f) && filter.Explosion->HasReachedEnd) {
				f.Add<ClearGrid>(filter.Entity);
				return;
			}	

			if (filter.Timer->HasExpired(f))
			{
				var explosionOrigin = f.Unsafe.GetPointer<Transform2D>(filter.Entity)->Position;
				ExtendExplosionSpread(f, filter.Explosion, explosionOrigin);
				filter.Timer->SetFromTime(f, filter.Explosion->CellSpreadTime);
			}
		}
	
		private void ExtendExplosionSpread(Frame f, Explosion* explosion, FPVector2 explosionOrigin)
		{
			var position = default(FPVector2);

			for (var i = 0; i < Constants.CARDINAL_DIRECTION_COUNT; i++)
			{
				if (explosion->DirectionBlocked[i]) continue;

				position = explosionOrigin;
				
				var direction = Direction_ext.ConvertIntToDirection(i);
				var distanceMultiplayer = explosion->CurrentReach + 1;

				switch (direction)
				{
					case Direction.Right:
						position += FPVector2.Right * distanceMultiplayer;
						break;
					case Direction.Left:
						position += FPVector2.Left * distanceMultiplayer;
						break;
					case Direction.Up:
						position += FPVector2.Up * distanceMultiplayer;
						break;
					case Direction.Down:
						position += FPVector2.Down * distanceMultiplayer;
						break;
				}

				var neighborCell = f.Grid.GetCellPtr(position);

				if (neighborCell->IsEmpty || neighborCell->IsBurnable) {
					explosion->ReachDirection[i]++;
				}
				
				if (neighborCell->IsBlocking) {
					explosion->DirectionBlocked[i] = true;
				}
			}
			
			explosion->CurrentReach++;
			
			explosion->AllDirectionsBlocked = true;
			for (var i = 0; i < Constants.CARDINAL_DIRECTION_COUNT; i++)
			{
				if (explosion->DirectionBlocked[i]) continue;
				
				explosion->AllDirectionsBlocked = false;
			}
		}
	}
}
