namespace Quantum
{
	public unsafe class InputSystem : SystemMainThreadFilter<InputSystem.InputFilter>
	{
		public struct InputFilter
		{
			public EntityRef Entity;
			public PlayerLink* PlayerLink;
		}

		public override void Update(Frame f, ref InputFilter filter)
		{
			if (f.Unsafe.GetPointerSingleton<GameSession>()->State != GameSessionState.Playing) return;
		
			Input* input = f.GetPlayerInput(filter.PlayerLink->Id);
			ConstructMovement(f, input, filter.Entity);
			ConstructBombPlacement(f, input, filter.Entity);
		}

		private void ConstructMovement(Frame f, Input* input, EntityRef entity)
		{
			var movement = f.Unsafe.GetPointer<Movement>(entity);
			var previousDirection = movement->CurrentInput;

			var newlyPressedDirections = input->GetNewDirections();
			var pressedDirections = input->GetPressedDirections();

			var horizontalMovement = newlyPressedDirections.ExtractHorizontalDirections();
			var verticalMovement = newlyPressedDirections.ExtractVerticalDirections();

			if (horizontalMovement == default)
			{
				var previousHorizontalMovement = previousDirection.ExtractHorizontalDirections();
				if (pressedDirections.HasFlag(previousHorizontalMovement))
				{
					horizontalMovement = previousHorizontalMovement;
				}

				if (horizontalMovement == default)
				{
					horizontalMovement = pressedDirections.ExtractHorizontalDirections();
				}
			}

			if (verticalMovement == default)
			{
				var previousVerticalMovement = previousDirection.ExtractVerticalDirections();
				if (pressedDirections.HasFlag(previousVerticalMovement))
				{
					verticalMovement = previousVerticalMovement;
				}

				if (verticalMovement == default)
				{
					verticalMovement = pressedDirections.ExtractVerticalDirections();
				}
			}

			var currentDirection = horizontalMovement ^ verticalMovement;

			movement->CurrentInput = currentDirection;
			movement->LastNewInput = newlyPressedDirections;
		}

		private void ConstructBombPlacement(Frame f, Input* input, EntityRef entity)
		{
			var abilityPlaceBomb = f.Unsafe.GetPointer<AbilityPlaceBomb>(entity);
			abilityPlaceBomb->WantsToPlaceBomb = input->PlaceBomb.WasPressed;
		}
	}
}