using System;
using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;

public class BlockDestroyableViewSetup : MonoBehaviour
{
    [SerializeField] private BlockDestroyableAnimation _blockDestroyableAnimation = null;
    
    private EntityRef _entityRef = EntityRef.None;

    private void OnEnable()
    {
        QuantumEvent.Subscribe<EventOnBurnBlock>(this, eventData => BurnBlock(eventData.entityRef));
    }

    private void OnDisable()
    {
        QuantumEvent.UnsubscribeListener<EventOnBurnBlock>(this);
    }

    public void OnEntityInstantiated()
    {
        SetupView();
    }

    private void SetupView()
    {
        _entityRef = GetComponent<EntityView>().EntityRef;
        
        var frame = QuantumRunner.Default.Game.Frames.Predicted;
        var blockDestroyable = frame.Get<BlockDestroyable>(_entityRef);
        _blockDestroyableAnimation.InitAnimation(blockDestroyable.OnExplosionTTL.AsFloat);

        if (frame.Has<Timer>(_entityRef) == false) return;
        
        BurnBlock(_entityRef);
    }

    private void BurnBlock(EntityRef entityRef)
    {
        if (_entityRef != entityRef) return;
        var frame = QuantumRunner.Default.Game.Frames.Predicted;
        
        if (frame.TryGet<Timer>(_entityRef, out var timer) == false) return;

        StartCoroutine(_blockDestroyableAnimation.ShrinkAnimation(timer.GetRemainingTime(frame).AsFloat));

    }
}
