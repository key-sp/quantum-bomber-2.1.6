using Photon.Deterministic;

namespace Quantum {
  unsafe partial class Frame
  {
    public Grid Grid = null;

    // Called once when the frame is initially created.
    partial void InitUser()
    {
      Grid = new Grid();
      Grid.Init(this);
    }

    // Called when the Frame is destroyed.
    partial void FreeUser()
    {
      Grid.Clear(this);
    }

    // Called when the previous frame state is copied into the next frame.
    partial void CopyFromUser(Frame frame)
    {
      Grid.CopyFromUser(frame.Grid);
    }

    // Called when the frame is serialized (e.g. as a buddy snapshot for late joiners).
    partial void SerializeUser(FrameSerializer serializer)
    {
      Grid.SerializeUser(serializer);
    }

    // Called when a desync happens.
    partial void DumpFrameUser(ref string dump)
    {
      Grid.DumpFrameUser(ref dump);
    }

    public bool FlipCoin()
    {
      return RNG->Next() > FP._0_50;
    }
  }
}
