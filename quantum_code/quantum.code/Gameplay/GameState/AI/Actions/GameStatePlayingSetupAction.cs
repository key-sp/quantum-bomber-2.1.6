using System;
using System.Runtime.InteropServices;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]
  [AssetObjectConfig(GenerateLinkingScripts = true, GenerateAssetCreateMenu = false, GenerateAssetResetMethod = false)]
  public partial class GameStatePlayingSetupAction : AIAction
  {
    public override unsafe void Update(Frame f, EntityRef entity, ref AIContext aiContext)
    {
      GameSession* gameSession = f.Unsafe.GetPointer<GameSession>(entity);
      gameSession->State = GameSessionState.Playing;
      Timer* timer = f.Unsafe.GetPointer<Timer>(entity);
      timer->Reset();
    }
  }
}
