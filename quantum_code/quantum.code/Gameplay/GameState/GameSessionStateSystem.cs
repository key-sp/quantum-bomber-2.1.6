namespace Quantum
{
  public unsafe class GameSessionStateSystem : SystemMainThreadFilter<GameSessionStateSystem.Filter>
  {
    public struct Filter
    {
      public EntityRef Entity;
      public GameSession* GameSession;
      public Timer* Timer;
      public HFSMAgent* Agent;
    }

    public override void OnInit(Frame f)
    {
      var entity = f.Create(f.RuntimeConfig.GameStatePrototype);
      HFSMAgent* agent = f.Unsafe.GetPointer<HFSMAgent>(entity);
      HFSMRoot hfsmRoot = f.FindAsset<HFSMRoot>(agent->Data.Root.Id);
      HFSMManager.Init(f, &agent->Data, entity, hfsmRoot);

    }

    public override void Update(Frame f, ref Filter filter)
    {
      HFSMManager.Update(f, f.DeltaTime, filter.Entity);
    }
  }
}