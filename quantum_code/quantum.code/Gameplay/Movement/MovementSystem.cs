using Photon.Deterministic;

namespace Quantum
{
	public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.MovementFilter>
	{
		public struct MovementFilter
		{
			public EntityRef Entity;
			public Transform2D* Transform;
			public Movement* Movement;
		}

		public override void Update(Frame frame, ref MovementFilter filter)
		{
			var inputDirection = filter.Movement->CurrentInput;
			var lastPressedInput = filter.Movement->LastNewInput;
			
			var config = frame.FindAsset<MovementConfig>(filter.Movement->Config.Id);

			var moveResult = GetMovementResult(frame, ref filter, inputDirection, lastPressedInput, config);
			
			filter.Movement->IsMoving = moveResult.Direction != default;
			filter.Movement->MoveDirection = moveResult.Direction;

			UpdateCurrentSpeed(frame, filter.Movement, inputDirection, ref moveResult, config);
			UpdateMovement(frame, ref filter, ref moveResult);
			UpdateRotation(frame, ref filter, ref moveResult, inputDirection, lastPressedInput, config);
		}

		private static MoveResult GetMovementResult(Frame frame, ref MovementFilter filter, Direction inputDirection, Direction lastPressedInput, MovementConfig config)
		{
			var moveResult = default(MoveResult);
			var position = filter.Transform->Position;
			var gridPosition = position.RoundToInt();
			
			// TODO: Review this comment
			// Try last pressed input first, but only when not trying to do diagonal movement (= both input axis pressed)
			if (lastPressedInput != default && inputDirection.IsDirectionSingleAxis()) {
				moveResult = GetMoveVector(frame, position, gridPosition, lastPressedInput, filter.Movement);
			}

			if (moveResult.IsValid == false) {
				moveResult = GetMoveVector(frame, position, gridPosition, inputDirection, filter.Movement);
			}
			
			return moveResult;
		}

		private static void UpdateCurrentSpeed(Frame frame, Movement* movement, Direction input, ref MoveResult result, MovementConfig config)
		{
			if (result.IsValid && input != default)
			{
				// TODO: Account for "slow down" sickness / power up
				var speedProgress = (movement->MaxSpeed - config.DefaultSpeed) / (config.MaxSpeed - config.DefaultSpeed);
				var currentAcceleration = config.Acceleration + config.Acceleration * speedProgress; 
				var delta = currentAcceleration * frame.DeltaTime;
				movement->CurrentSpeed = movement->CurrentSpeed < movement->MaxSpeed ? movement->CurrentSpeed + delta : movement->MaxSpeed;
			}
			else
			{
				var delta = config.Braking * frame.DeltaTime;
				movement->CurrentSpeed = movement->CurrentSpeed > FP.EN2 ? movement->CurrentSpeed - delta : FP._0;
			}
		}

		private static void UpdateMovement(Frame frame, ref MovementFilter filter, ref MoveResult moveResult)
		{
			var movement = filter.Movement;
			var transform = filter.Transform;
			
			var moveDirection = moveResult.Direction;

			var position = transform->Position;
			var gridPosition = position.RoundToInt();
			
			if (moveDirection.X != FP._0) {
				// Snap Y
				position.Y = gridPosition.Y;
			}
			else if (moveDirection.Y != FP._0) {
				// Snap X
				position.X = gridPosition.X;
			}

			var velMoveDistance = movement->CurrentSpeed * frame.DeltaTime;
			var moveDelta = velMoveDistance < moveResult.MaxDistance ? 
				velMoveDistance : moveResult.MaxDistance;
			transform->Position = position + moveDirection * moveDelta;

			// When grid position changed, save if it was by vertical or horizontal move
			// to allow axis switching (= diagonal movement)
			if (transform->Position.RoundToInt() != gridPosition)
			{
				movement->LastMoveWasHorizontal = moveDirection.X != FP._0;
			}
		}

		private static void UpdateRotation(Frame frame, ref MovementFilter filter, ref MoveResult moveResult, Direction inputDirection, Direction lastPressedInput, MovementConfig config)
		{
			if (moveResult.IsValid)
			{
				SetRotation(frame, ref filter, moveResult.LookDirection, moveResult.RotationTimeMultiplier, config);
			}
			else
			{
				var lookDirection = lastPressedInput == default ? inputDirection : lastPressedInput;
				// Try at least look in the desired direction
				SetRotation(frame, ref filter, lookDirection.ConvertToVector(), FP._1, config);
			}

			var movement = filter.Movement;
			var transform = filter.Transform;
			
			var time = (frame.Number - movement->RotationStartTick) * frame.DeltaTime;
			if (time > movement->RotationDuration) {
				transform->Rotation = movement->TargetRotation;
			} else {
				var progress = time / movement->RotationDuration;
				transform->Rotation = FPMath.LerpRadians(movement->StartRotation, movement->TargetRotation, progress);
			}
		}
		
		private static void SetRotation(Frame frame, ref MovementFilter filter, FPVector2 lookDirection, FP rotationTimeMultiplier, MovementConfig config)
		{
			if (lookDirection == default) return;

			var movement = filter.Movement;
			var transform = filter.Transform;
			
			var targetRotation = FPMath.Atan2(lookDirection.Y, lookDirection.X);

			if (movement->TargetRotation == targetRotation && movement->RotationTimeMultiplier == rotationTimeMultiplier)
				return;

			movement->RotationTimeMultiplier = rotationTimeMultiplier;

			movement->StartRotation = transform->Rotation;
			movement->TargetRotation = targetRotation;

			var rotationDiff = FPMath.Abs(FPMath.AngleBetweenRadians(transform->Rotation, targetRotation) * FP.Rad2Deg);
			
			// TODO: Review equation to handle "movement slowdown".
			var speedProgress = (movement->MaxSpeed - config.DefaultSpeed) / (config.MaxSpeed - config.DefaultSpeed);
			var rotationTime = FPMath.Lerp(config.DefaultSpeedRotationSpeed, config.MaxSpeedRotationTime, speedProgress);
			
			movement->RotationStartTick = frame.Number;
			// TODO: Review the hardcode 90
			movement->RotationDuration = FPMath.Lerp(FP.EN2, rotationTime, rotationDiff / 90) * rotationTimeMultiplier;
		} 

		private static MoveResult GetMoveVector(Frame frame, FPVector2 position, FPVector2 gridPosition, Direction direction, Movement* movement)
		{
			// TODO: account for corner slide toggle
			// TODO: account for diagonal movement
			var config = frame.FindAsset<MovementConfig>(movement->Config.Id);
			
			var result = default(MoveResult);
			var (horizontalMove, verticalMove) = direction.ExtractDirections();

			// Switch order of checks to allow diagonal movement
			var tryVerticalFirst = movement->LastMoveWasHorizontal;

			var firstDirection = tryVerticalFirst ? verticalMove : horizontalMove;
			var secondDirection = tryVerticalFirst ? horizontalMove : verticalMove;
			
			if(firstDirection != default && CanMoveInDirection(frame, position, gridPosition, firstDirection.ConvertToVector(), movement, out result))
				return result;

			if(secondDirection != default && CanMoveInDirection(frame, position, gridPosition, secondDirection.ConvertToVector(), movement, out result))
				return result;

			return result;
		}

		private static bool CanMoveInDirection(Frame frame, FPVector2 position, FPVector2 gridPosition,
			FPVector2 desiredDirection, Movement* movement, out MoveResult result)
		{
			var directionToGrid = gridPosition - position;

			var otherAxisRemainder = FPMath.Abs(desiredDirection.X != FP._0 ? directionToGrid.Y : directionToGrid.X);
			
			if (IsCellWalkable(frame, gridPosition + desiredDirection))
			{
				if (otherAxisRemainder > FP.EN2) // Alignment threshold
				{
					// Align to grid first
					result = new MoveResult(GetDirectionVector(directionToGrid), otherAxisRemainder, desiredDirection, FP._2);
					return true;
				}
				
				// Next cell is free, let's return full move vector
				result = new MoveResult(desiredDirection, FP._1);
				return true;
			}

			var directionToGridLocal = FPVector2.Scale(directionToGrid, desiredDirection);
			var remainingDistance = FPMath.Max(directionToGridLocal.X, directionToGridLocal.Y);

			if (remainingDistance > FP._0)
			{
				// There is still some space left to arrive to grid position
				result = new MoveResult(desiredDirection, remainingDistance);
				return true;
			}

			// Get slide direction based on the offset from the grid, otherwise take the rotation direction
			var slideDirection = otherAxisRemainder > FP._0 ? GetDirectionVector(-directionToGrid) : GetRotationDirection(movement->TargetRotation);

			// Try slide
			if (slideDirection != -movement->MoveDirection // Do not slide to the opposite of movement direction
			    && IsCellWalkable(frame, gridPosition + slideDirection) // Cell to slide to first
			    && IsCellWalkable(frame, gridPosition + slideDirection + desiredDirection)) // Desired cell
			{
				result = new MoveResult(slideDirection, FP._1, desiredDirection, FP._2);
				return true;
			}

			result = default;
			return false;
		}

		private static bool IsCellWalkable(Frame frame, FPVector2 cellPosition)
		{
			return frame.Grid.GetCellPtr(cellPosition)->IsWalkable;
		}

		private static FPVector2 GetDirectionVector(FPVector2 vector)
		{
			if (vector == default) return default;

			if (FPMath.Abs(vector.X) > FPMath.Abs(vector.Y)) {
				return vector.X > FP._0 ? FPVector2.Right : FPVector2.Left;
			}

			return vector.Y > FP._0 ? FPVector2.Up : FPVector2.Down;
		}

		private static FPVector2 GetRotationDirection(FP rotation)
		{
			return new FPVector2(FPMath.Cos(rotation), FPMath.Sin(rotation));
		}
	}
}
