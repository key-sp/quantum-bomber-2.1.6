namespace Quantum
{
	public partial struct Cell
	{
		private const CellType BurnableType = CellType.BlockDestroyable | CellType.Bomb | CellType.Explosion | CellType.PowerUp;
		private const CellType BlockingType = CellType.BlockFix | CellType.BlockDestroyable | CellType.Bomb;
		private const CellType WalkableType = CellType.Empty | CellType.Explosion | CellType.PowerUp;

		public bool IsEmpty    => Type == CellType.Empty;
		public bool IsFix      => Type == default || (Type & CellType.BlockFix) > 0;
		public bool IsBurning  => (Type & CellType.Explosion) > 0;
		public bool HasPowerUp => (Type & CellType.PowerUp) > 0;
		public bool IsBurnable => (Type & BurnableType) > 0;
		public bool IsBlocking => Type == default || (Type & BlockingType) > 0;
		public bool IsWalkable => (Type & WalkableType) == Type;
		
		public void Add(CellType type)    => Type = Type.SetFlag(type);
		public void Set(CellType type)    => Type = type;
		public void Remove(CellType type) => Type = Type.ClearFlag(type);
	}
}
