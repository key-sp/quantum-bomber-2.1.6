using System;
using System.Runtime.InteropServices;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]
  [AssetObjectConfig(GenerateLinkingScripts = true, GenerateAssetCreateMenu = false, GenerateAssetResetMethod = false)]
  public partial class GameStateEndingSetupAction : AIAction
  {
    public override unsafe void Update(Frame f, EntityRef entity, ref AIContext aiContext)
    {
      GameSession* gameSession = f.Unsafe.GetPointer<GameSession>(entity);
      gameSession->State = GameSessionState.Ending;
      // There is only a single player left when this runs.
      var players = f.GetComponentIterator<PlayerLink>();
      foreach (var (_, playerLink) in players)
      {
        gameSession->Winner = playerLink.Id;
      }
      Timer* timer = f.Unsafe.GetPointer<Timer>(entity);
      timer->SetFromTime(f, gameSession->TimeUntilDisconnect);
    }
  }
}
