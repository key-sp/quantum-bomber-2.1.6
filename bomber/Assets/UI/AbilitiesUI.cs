using System;
using System.Collections;
using System.Collections.Generic;
using Quantum;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AbilitiesUI : IUiElement
{
    [SerializeField] private TextMeshProUGUI _bombAmount = null;
    [SerializeField] private TextMeshProUGUI _explosionReach = null;
    
    public override void UpdateUi(Frame frame)
    {
        var game = QuantumRunner.Default.Game;
        var localPlayer = game.GetLocalPlayers()[0];

        var filter = frame.Filter<PlayerLink, AbilityPlaceBomb>();
        while( filter.Next(out _, out var playerLink, out var bombAbility))
        {
            if (playerLink.Id != localPlayer) continue;
            _bombAmount.text = $"Bombs: {bombAbility.BombsAmount}";
            _explosionReach.text = $"Reach: {bombAbility.BombReach}";
        }
    }
}
