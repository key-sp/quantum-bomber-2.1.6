namespace Quantum
{
	public unsafe class PowerUpSystem : SystemMainThreadFilter<PowerUpSystem.PowerUpFilter>
	{
		public struct PowerUpFilter
		{
			public EntityRef Entity;
			public PowerUp* PowerUp;
			public Transform2D* Transform;
		}

		public override void Update(Frame f, ref PowerUpFilter filter)
		{
			// --- Explosion Damage
			if(f.Grid.GetCellPtr(filter.Transform->Position)->IsBurning) {
				f.Add<ClearGrid>(filter.Entity);
				return;
			}

			// --- Player Overlap
			if (IsTouchingPlayer(f, filter.Transform, out var playerEntityRef))
			{
				ConsumePowerUp(f, filter.PowerUp, playerEntityRef);
				f.Add<ClearGrid>(filter.Entity);
			}
		}

		private bool IsTouchingPlayer(Frame f, Transform2D* powerUpTransform, out EntityRef playerEntityRef)
		{
			var playerFilter = f.Filter<Transform2D, PlayerLink>();

			while (playerFilter.Next(out playerEntityRef, out var playerTransform, out _))
			{
				var playerPos = playerTransform.Position.RoundToInt(Axis.Both);

				if (playerPos.X != powerUpTransform->Position.X) continue;
				if (playerPos.Y != powerUpTransform->Position.Y) continue;

				return true;
			}
			
			playerEntityRef = EntityRef.None;

			return false;
		}

		private void ConsumePowerUp(Frame f, PowerUp* powerUp, EntityRef playerEntityRef)
		{
			switch (powerUp->Type)
			{
				case PowerUpType.BombAmount:
				{
					var bombAbility = f.Unsafe.GetPointer<AbilityPlaceBomb>(playerEntityRef);
					bombAbility->ModifyBombAmount(f, powerUp->Amount, powerUp->MaxOutValue);
					break;
				}
				case PowerUpType.ExplosionReach:
				{
					var bombAbility = f.Unsafe.GetPointer<AbilityPlaceBomb>(playerEntityRef);
					bombAbility->ModifyBombPower(f, powerUp->Amount, powerUp->MaxOutValue);
					break;
				}
				case PowerUpType.MovementSpeed:
				{
					var movement = f.Unsafe.GetPointer<Movement>(playerEntityRef);
					movement->ModifySpeed(f, powerUp->Amount, powerUp->MaxOutValue);
					break;
				}
				case PowerUpType.Curse:
					break;
				default:
					Log.Warn($"Power Up of type {powerUp->Type} is not implemented in PowerUpSystem.");
					break;
			}
		}
	}
}
