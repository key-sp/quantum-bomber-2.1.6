using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAnimation : MonoBehaviour
{
    [SerializeField] private float _lerpTime = 1.0f;
    [Range(0.0f, 1.0f)] [SerializeField] private float _maxSize = 1.0f;
    [Range(0.0f, 1.0f)] [SerializeField] private float _minSize = 0.5f;

    [SerializeField] private AnimationCurve _animationCurve = default;

    private bool _isScalingUp = false;

    private float _currentLerpTime = default;
    private Vector3 _originalScale = Vector3.zero;
    private Vector3 _currentScaleTarget = Vector3.zero;

    private Transform _transform = null;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _originalScale = _transform.localScale;
    }

    public void StartAnimation()
    {
        _isScalingUp = false;
        ResetLerp();
    }

    private void ResetLerp()
    {
        _currentLerpTime = 0.0f;

        var scalingFactor = _isScalingUp ? _maxSize : _minSize;
        _currentScaleTarget = scalingFactor * _originalScale;
    }
    
    void Update()
    {
        _currentLerpTime += Time.deltaTime;
        
        var time = _currentLerpTime / _lerpTime;
        var modifier = _animationCurve.Evaluate(time);
        
        _transform.localScale = Vector3.Lerp(transform.localScale, _currentScaleTarget, modifier);
        
        if (_currentLerpTime < _lerpTime) return;

        _isScalingUp = !_isScalingUp;

        ResetLerp();
    }
}
