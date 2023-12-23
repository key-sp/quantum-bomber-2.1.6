using Photon.Deterministic;

namespace Quantum
{
	public unsafe class AbilityPlaceBombSystem : SystemMainThreadFilter<AbilityPlaceBombSystem.AbilityFilter>
	{
		public struct AbilityFilter
		{
			public EntityRef Entity;
			public AbilityPlaceBomb* AbilityPlaceBomb;
		}

		public override void Update(Frame f, ref AbilityFilter filter)
		{
			// --- Check Player Related conditions
			if (filter.AbilityPlaceBomb->WantsToPlaceBomb == false) return;
			if (filter.AbilityPlaceBomb->CanPlaceBomb == false) return;
			
			// --- Check if cell is free
			var playerPosition = f.Unsafe.GetPointer<Transform2D>(filter.Entity)->Position;
			var bombPosition = playerPosition.RoundToInt(Axis.Both);
			
			if (f.Grid.GetCellPtr(bombPosition)->IsBlocking) return;
			
			// --- Execute Ability
			CreateAndPlaceBomb(f, ref filter, bombPosition);

			filter.AbilityPlaceBomb->BombPlaced();
		}

		private void CreateAndPlaceBomb(Frame f, ref AbilityFilter filter, FPVector2 bombPosition)
		{
			var playerLink = f.Unsafe.GetPointer<PlayerLink>(filter.Entity);
			
			var bombEntity = f.Create(f.RuntimeConfig.DefaultBombPrototype);
			
			var transform = f.Unsafe.GetPointer<Transform2D>(bombEntity);
			transform->Position = bombPosition;
			
			var bomb = f.Unsafe.GetPointer<Bomb>(bombEntity);
			
			bomb->OwnerId = playerLink->Id;
			bomb->OwnerRef = filter.Entity;
			bomb->Reach = filter.AbilityPlaceBomb->BombReach;
		}
	}
}
