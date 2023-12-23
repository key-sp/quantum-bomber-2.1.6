namespace Quantum
{

  public partial struct Input
  {
    public Direction GetNewDirections()
    {
      var newlyPressedDirections = default(Direction);
      if (MoveDown.WasPressed)
      {
        newlyPressedDirections = newlyPressedDirections.SetFlag(Direction.Down);
      }

      if (MoveUp.WasPressed)
      {
        newlyPressedDirections = newlyPressedDirections.SetFlag(Direction.Up);
      }

      if (MoveLeft.WasPressed)
      {
        newlyPressedDirections = newlyPressedDirections.SetFlag(Direction.Left);
      }

      if (MoveRight.WasPressed)
      {
        newlyPressedDirections = newlyPressedDirections.SetFlag(Direction.Right);
      }

      return newlyPressedDirections;
    }

    public Direction GetPressedDirections()
    {
      var pressedDirections = default(Direction);
      if (MoveDown.IsDown)
      {
        pressedDirections = pressedDirections.SetFlag(Direction.Down);
      }

      if (MoveUp.IsDown)
      {
        pressedDirections = pressedDirections.SetFlag(Direction.Up);
      }

      if (MoveLeft.IsDown)
      {
        pressedDirections = pressedDirections.SetFlag(Direction.Left);
      }

      if (MoveRight.IsDown)
      {
        pressedDirections = pressedDirections.SetFlag(Direction.Right);
      }

      return pressedDirections;
    }
  }
}