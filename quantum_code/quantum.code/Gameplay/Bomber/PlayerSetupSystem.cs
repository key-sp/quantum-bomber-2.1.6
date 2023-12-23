namespace Quantum
{
	public unsafe class PlayerSetupSystem : SystemSignalsOnly, ISignalOnPlayerConnected,  ISignalOnPlayerDisconnected, ISignalOnPlayerDataSet
	{
		public void OnPlayerConnected(Frame f, PlayerRef player)
		{
			CreatePlayerCharacter(f, player);
		}

		public void OnPlayerDisconnected(Frame f, PlayerRef player)
		{
			foreach (var playerLink in f.GetComponentIterator<PlayerLink>())
			{
				if (playerLink.Component.Id != player) continue;

				f.Destroy(playerLink.Entity);
			}
		}

		public void OnPlayerDataSet(Frame f, PlayerRef player)
		{
			f.Events.OnUpdatePlayerColor(player);
		}

		private void CreatePlayerCharacter(Frame f, PlayerRef player)
		{
			Log.Debug($"Created Character for Player {player}");
		
			var entityRef = f.Create(f.RuntimeConfig.DefaultBomberPrototype);
			var playerLink = f.Unsafe.GetPointer<PlayerLink>(entityRef);
			playerLink->Id = player;

			var transform2D = f.Unsafe.GetPointer<Transform2D>(entityRef);
			var spawnPoints = f.ResolveList(f.Global->SpawnPoints);
		
			// Modulo to prevent overflow in case not enough spawn points could be generated.
			transform2D->Position = spawnPoints[player._index % spawnPoints.Count];

			// TODO: Use OnComponentAdded in MovementSystem to Init? 
			var movement = f.Unsafe.GetPointer<Movement>(entityRef);
			var movementConfig = f.FindAsset<MovementConfig>(movement->Config.Id);
			movement->MaxSpeed = movementConfig.DefaultSpeed;

			// TODO: Use OnComponentAdded in AbilityBombSystem to Init?
			var bombAbility = f.Unsafe.GetPointer<AbilityPlaceBomb>(entityRef);
			var bombAbilityConfig = f.FindAsset<AbilityPlaceBombConfig>(bombAbility->Config.Id);
			bombAbility->BombsAmount = bombAbilityConfig.BombsAmountDefault;
			bombAbility->BombReach = bombAbilityConfig.BombsReachDefault;
		}
	}
}