using System;
using System.Runtime.InteropServices;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]
  [AssetObjectConfig(GenerateLinkingScripts = true, GenerateAssetCreateMenu = false, GenerateAssetResetMethod = false)]
  public partial class GameStateSetupAction : AIAction
  {
    public override unsafe void Update(Frame f, EntityRef entity, ref AIContext aiContext)
    {
      var gameSession = f.Unsafe.GetPointer<GameSession>(entity);
      gameSession->State = GameSessionState.Starting;

      var timer = f.Unsafe.GetPointer<Timer>(entity);
      timer->SetFromTime(f, gameSession->TimeUntilStart);
    }
  }
}
