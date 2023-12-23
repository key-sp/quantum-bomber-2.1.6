using Photon.Deterministic;

namespace Quantum
{
	public static unsafe partial class Direction_ext {

		public static FPVector2 ConvertToVector(this Direction direction)
		{
			var directionVector = direction switch
			{
				Direction.Right => FPVector2.Right,
				Direction.Left => FPVector2.Left,
				Direction.Up => FPVector2.Up,
				Direction.Down => FPVector2.Down,
				_ => FPVector2.Zero
			};

			return directionVector;
		}

		public static FPQuaternion ConvertToQuaternion(this Direction direction)
		{
			var directionAngle = direction switch
			{
				Direction.Right => FPQuaternion.Euler(0, 0, 90),
				Direction.Left => FPQuaternion.Euler(0, 0, -90),
				Direction.Up => FPQuaternion.Identity,
				Direction.Down => FPQuaternion.Euler(0, 0, 180),
				_ => FPQuaternion.Identity
			};

			return directionAngle;
		}
		
		public static QTuple<Direction, Direction> ExtractDirections(this Direction self)
		{
			return new QTuple<Direction, Direction>(self.ExtractHorizontalDirections(), self.ExtractVerticalDirections());
		}
		
		public static bool IsDirectionSingleAxis(this Direction direction)
		{
			if (direction == default) return false;
			if ((direction & (Direction.Left | Direction.Right)) == direction) return true;

			return (direction & (Direction.Up | Direction.Down)) == direction;
		}
		
		public static Direction ExtractHorizontalDirections(this Direction self)
		{
			var directions = self & ~(Direction.Up | Direction.Down);
			return directions;
		}

		public static Direction ExtractVerticalDirections(this Direction self)
		{
			var directions = self & ~(Direction.Left | Direction.Right);
			return directions;
		}

		// Regular conversion
		public static Direction ConvertIntToDirection(int value) {
			return (Direction) (1 << value);
		}

		public static int ConvertToIndex(this Direction self)
		{
			return (int)self / 2;
		}

		// Assumes 0-based
		public static Direction ConvertToDirectionWithOffset(int value, int offsetFromZero) {
			return (Direction)(1 << (value + offsetFromZero));
		}
	}
}