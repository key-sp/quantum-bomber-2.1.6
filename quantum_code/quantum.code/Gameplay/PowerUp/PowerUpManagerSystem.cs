using Photon.Deterministic;

namespace Quantum
{
	public unsafe class PowerUpManagerSystem : SystemMainThread, ISignalOnComponentAdded<PowerUpManager>, ISignalOnComponentRemoved<PowerUpManager>
	{
		public override void OnInit(Frame f)
		{
			f.Create(f.RuntimeConfig.PowerUpManagerPrototype);
		}

		public override void Update(Frame f)
		{
			var manager = f.Unsafe.GetPointerSingleton<PowerUpManager>();

			var spawnPositions = f.ResolveList(manager->SpawnPositions);
			
			if (spawnPositions.Count > 0 == false) return;

			for (var i = 0; i < spawnPositions.Count; i++)
			{
				if (f.Grid.GetCellPtr(spawnPositions[i])->IsBlocking) continue;
				if (f.Grid.GetCellPtr(spawnPositions[i])->IsBurning) continue;

				if (TryGetRandomPowerUp(f, manager, out var powerUpPrototype))
				{
					var entityRef = f.Create(powerUpPrototype);
					var transform = f.Unsafe.GetPointer<Transform2D>(entityRef);

					transform->Position = spawnPositions[i];
				}
				
				spawnPositions.RemoveAt(i);
				i--;
			}
		}

		private bool TryGetRandomPowerUp(Frame f, PowerUpManager* manager, out AssetRefEntityPrototype powerUpAssetRef)
		{
			powerUpAssetRef = default;
			if (manager->PowerUpPrototypes == default) return false;

			var probability = f.RNG->NextInclusive(FP._0, FP._100);
			if (probability > manager->SpawnProbability) return false;
			
			var list = f.ResolveList(manager->PowerUpPrototypes);

			var index = f.RNG->Next(0, list.Count);

			powerUpAssetRef = list[index];
			return true;
		}
		
		public void OnAdded(Frame f, EntityRef entity, PowerUpManager* manager)
		{
			manager->SpawnPositions = f.AllocateList<FPVector2>();
		}

		public void OnRemoved(Frame f, EntityRef entity, PowerUpManager* manager)
		{
			f.FreeList(manager->SpawnPositions);
			manager->SpawnPositions = default;
			
			f.FreeList(manager->PowerUpPrototypes);
			manager->PowerUpPrototypes = default;
		}
	}
}
