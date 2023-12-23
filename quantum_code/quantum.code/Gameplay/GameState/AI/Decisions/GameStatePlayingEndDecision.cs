
using System;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]
  [AssetObjectConfig(GenerateLinkingScripts = true, GenerateAssetCreateMenu = false, GenerateAssetResetMethod = false)]
  public partial class GameStatePlayingEndDecision : HFSMDecision
  {
    public override unsafe bool Decide(Frame f, EntityRef entity, ref AIContext aiContext)
    {
#if DEBUG
      if (f.RuntimeConfig.IsOnePlayerTest) return false;
#endif
      // <= 1 accounts for situations where all last standing players died during the same frame
      return f.ComponentCount<PlayerLink>() <= 1;
    }
  }
}
