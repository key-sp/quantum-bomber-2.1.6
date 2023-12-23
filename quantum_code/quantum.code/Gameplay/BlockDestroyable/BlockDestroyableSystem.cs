namespace Quantum
{
	public unsafe class BlockDestroyableSystem : SystemMainThreadFilter<BlockDestroyableSystem.BlockDestroyableFilter>
	{
		public struct BlockDestroyableFilter
		{
			public EntityRef Entity;
			public Transform2D* Transform;
			public BlockDestroyable* BlockDestroyable;
		}

		public override void OnInit(Frame f)
		{
			// Create Destructible Blocks
			
			var width = f.Grid.GetGridWidth();
			var height = f.Grid.GetGridHeight();
			
			for (var x = 1; x < width - 1; x++)
			{
				for (var y = 1; y < height - 1; y++)
				{
					// Skip blocked cells
					if (x % 2 == 0 && y % 2 == 0) continue;
					
					var cell = f.Grid.GetCellPtr(x, y);

					// At this stage a cell on the playing field can only be
					// Free cell or a destroyable block  
					if (cell->IsWalkable) continue;

					var entityRef = f.Create(f.RuntimeConfig.DefaultBlockDestroyablePrototype);
					
					var transform = f.Unsafe.GetPointer<Transform2D>(entityRef);
					transform->Position.X = x;
					transform->Position.Y = y;
				}
			}
		}
		
		public override void Update(Frame f, ref BlockDestroyableFilter filter)
		{
			CheckForExplosionOverlap(f, ref filter);
			CheckForExpiredTtl(f, ref filter);
		}

		private void CheckForExplosionOverlap(Frame f, ref BlockDestroyableFilter filter)
		{
			if (f.Has<Timer>(filter.Entity)) return;
			
			var position = filter.Transform->Position;
			var cell = f.Grid.GetCellPtr(position);
			
			if (cell->IsBurning == false) return;
			
			f.Add<Timer>(filter.Entity);
			var timer = f.Unsafe.GetPointer<Timer>(filter.Entity);
			timer->SetFromTime(f, filter.BlockDestroyable->OnExplosionTTL);

			f.Events.OnBurnBlock(filter.Entity);
		}
		
		private void CheckForExpiredTtl(Frame f, ref BlockDestroyableFilter filter)
		{
			if (f.Unsafe.TryGetPointer<Timer>(filter.Entity, out var timer) == false) return;
			
			if (timer->HasExpired(f) == false) return;

			f.Add<ClearGrid>(filter.Entity);

			var powerUpManager = f.Unsafe.GetPointerSingleton<PowerUpManager>();
			powerUpManager->AddNewSpawnPosition(f, filter.Transform->Position);
		}
		
	}
}
