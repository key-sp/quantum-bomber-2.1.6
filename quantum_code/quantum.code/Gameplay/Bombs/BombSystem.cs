namespace Quantum
{
	public unsafe class BombSystem : SystemMainThreadFilter<BombSystem.BombFilter>
	{
		public struct BombFilter
		{
			public EntityRef EntityRef;
			public Transform2D* Transform;
			public Bomb* Bomb;
			public Timer* Timer;
		}

		public override void Update(Frame f, ref BombFilter filter)
		{
			if (filter.Timer->IsRunning == false) {
				LightBomb(f, ref filter);
			}

			if (f.Grid.GetCellPtr(filter.Transform->Position)->IsBurning) {
				filter.Timer->ExpiresImmediately(f);
			}
			
			if (filter.Timer->HasExpired(f)) {
				BlowUpBomb(f, ref filter);
				f.Add<ClearGrid>(filter.EntityRef);
			}
		}

		private void LightBomb(Frame f, ref BombFilter filter)
		{
			var config = f.FindAsset<BombConfig>(filter.Bomb->Config.Id);
			filter.Timer->SetFromTime(f, config.TimeToLiveInSeconds);
		}

		private void BlowUpBomb(Frame f, ref BombFilter filter)
		{
			var bombPosition = filter.Transform->Position;
			
			var entityRef = f.Create(f.RuntimeConfig.DefaultExplosionPrototype);

			var explosionOrigin = f.Unsafe.GetPointer<Transform2D>(entityRef);
			explosionOrigin->Position =  bombPosition;
			
			var explosion = f.Unsafe.GetPointer<Explosion>(entityRef);
			explosion->MaxReach = filter.Bomb->Reach;
			explosion->OwnerId = filter.Bomb->OwnerId;
			explosion->OwnerRef = filter.Bomb->OwnerRef;

			if (f.Exists(filter.Bomb->OwnerRef))
			{
				var bombAbility = f.Unsafe.GetPointer<AbilityPlaceBomb>(filter.Bomb->OwnerRef);
				bombAbility->BombExploded();
			}
		}
	}
}
