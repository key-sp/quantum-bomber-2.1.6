using System;
using System.Collections;
using Quantum;
using Quantum.Demo;
using TMPro;
using UnityEngine;

public class UiGameState : IUiElement
{
    [SerializeField] private GameObject _gameStateCanvas = null;
    [SerializeField] private TextMeshProUGUI _gameStateTMP = null;
    [SerializeField] private TextMeshProUGUI _gameStateMessageTMP = null;
    [SerializeField] private TextMeshProUGUI _timerTMP = null;
    
    // Update is called once per frame
    
    public override void UpdateUi(Frame frame)
    {
        if (frame.TryGetSingletonEntityRef<GameSession>(out var entity) == false) return;

        var timer = frame.Get<Timer>(entity);
        var gameSession = frame.GetSingleton<GameSession>();

        _gameStateCanvas.gameObject.SetActive(gameSession.State != GameSessionState.Playing);

        switch (gameSession.State)
        {
            case GameSessionState.Starting:
                var timeUntilGameStarts = timer.GetRemainingTime(frame).AsFloat;
                _timerTMP.text = $"Game starts in {(int)timeUntilGameStarts} seconds";
                _gameStateMessageTMP.text = "";
                _gameStateTMP.text = "Starting";
                break;
            case GameSessionState.Playing:
                break;
            case GameSessionState.Ending:
                _gameStateMessageTMP.text =
                    gameSession.Winner.IsValid ? $"Player {gameSession.Winner._index} won!" : "DRAW!";

                var timeUntilDisconnection = timer.GetRemainingTime(frame).AsFloat;

                // If more than 60 seconds are left until disconnection, write out counter in min + sec
                _timerTMP.text = timeUntilDisconnection > 60
                    ? $"Disconnection in {(int)timeUntilDisconnection / 60} min {(int)timeUntilDisconnection % 60} seconds"
                    : $"Disconnection in {(int)timeUntilDisconnection} seconds";

                _gameStateTMP.text = "Game Over";

                if (timer.HasExpired(frame))
                {
                    UIMain.Client.Disconnect();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
