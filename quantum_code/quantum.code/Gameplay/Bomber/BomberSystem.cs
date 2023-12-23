namespace Quantum
{
	public unsafe class BomberSystem : SystemMainThreadFilter<BomberSystem.BomberFilter>
	{
		public struct BomberFilter
		{
			public EntityRef Entity;
			public Bomber* Bomber;
			public Transform2D* Transform;
		}

		public override void Update(Frame f, ref BomberFilter filter)
		{
			var gridPosition = filter.Transform->Position.RoundToInt(Axis.Both);
			var isInvincible = false;
#if DEBUG
			// Used for debugging purposes
			isInvincible = f.RuntimeConfig.IsInvincible;	
#endif
			if (isInvincible == false && f.Grid.GetCellPtr(gridPosition)->IsBurning)
			{
				// Death animation is triggered from OnEntityDestroyed
				f.Destroy(filter.Entity);
			}
		}
	}
}
