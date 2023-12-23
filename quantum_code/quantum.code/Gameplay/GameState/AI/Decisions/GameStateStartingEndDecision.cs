
using System;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]
  [AssetObjectConfig(GenerateLinkingScripts = true, GenerateAssetCreateMenu = false, GenerateAssetResetMethod = false)]
  public partial class GameStateStartingEndDecision : HFSMDecision
  {
    public override unsafe bool Decide(Frame f, EntityRef entity, ref AIContext aiContext)
    {
      Timer* timer = f.Unsafe.GetPointer<Timer>(entity);
      return timer->HasExpired(f);
    }
  }
}
