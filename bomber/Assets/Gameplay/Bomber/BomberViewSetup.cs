using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Quantum;
using UnityEngine;

public class BomberViewSetup : MonoBehaviour
{
    [SerializeField] private BomberAnimationController _bomberAnimationController = null;
    [SerializeField] private BomberColorizer _bomberColorizer = null;
    
    [Tooltip("Delay until the View is destroyed after the Entity has ceased existing")]
    [SerializeField] private float _viewCleanUpDelay = 1.5f;

    private EntityRef _entityRef = EntityRef.None;

    private void OnEnable()
    {
        QuantumEvent.Subscribe<EventOnUpdatePlayerColor>(this, eventData => ColorizeBomber(eventData.playerRef));
    }

    private void OnDisable()
    {
        QuantumEvent.UnsubscribeListener<EventOnUpdatePlayerColor>(this);
    }

    public void OnEntityInstantiated()
    {
        SetupView();
    }

    public void OnEntityDestroyed()
    {
        DestroyView();
    }

    private void SetupView()
    {
        _entityRef = GetComponent<EntityView>().EntityRef;
        _bomberAnimationController.StartAnimation(_entityRef);
        
        var frame = QuantumRunner.Default.Game.Frames.Predicted;
        var playerRef = frame.Get<PlayerLink>(_entityRef).Id;
        
        ColorizeBomber(playerRef);
    }

    private void ColorizeBomber(PlayerRef playerRef)
    {
        var frame = QuantumRunner.Default.Game.Frames.Predicted;
        
        if (playerRef != frame.Get<PlayerLink>(_entityRef).Id) return;
        
        var runtimePlayer = frame.GetPlayerData(playerRef);
        
        if (runtimePlayer == null) return;
        
        _bomberColorizer.UpdateColor(runtimePlayer.Color.ToColor());
    }
    
    private void DestroyView()
    {
        var totalDelay = _bomberAnimationController.StartDeathRoutine(_viewCleanUpDelay);
        Destroy(gameObject, totalDelay);
    }
}
