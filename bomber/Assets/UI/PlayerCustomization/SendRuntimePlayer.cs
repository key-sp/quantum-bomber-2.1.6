using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;

public class SendRuntimePlayer : QuantumCallbacks
{
    public override void OnGameStart(Quantum.QuantumGame game) {
        // paused on Start means waiting for Snapshot
        if (game.Session.IsPaused) return;

        // TODO: Take into account multiple local players
        var localPlayer = game.GetLocalPlayers()[0];

        SendRuntimePlayerConfig(game, localPlayer);
    }

    public override void OnGameResync(QuantumGame game)
    {
        Debug.Log("Detected Resync. Verified tick: " + game.Frames.Verified.Number);
    }

    private void SendRuntimePlayerConfig(QuantumGame game, PlayerRef playerRef)
    {
        if (PlayerDataContainer.Instance == null) return;
        
        // TODO: Take into account multiple local players;
        
        game.SendPlayerData(playerRef, PlayerDataContainer.Instance.RuntimePlayer);
    }
}
