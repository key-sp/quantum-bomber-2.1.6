using System;
using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;

public class ExplosionPool : MonoBehaviour
{
    public static ExplosionPool Instance;

    [SerializeField] private ExplosionVisual _vfxPrefab = null;
    [SerializeField] private int _poolStartSize = 30;

    private List<ExplosionVisual> _availableExplosions;
    private List<ExplosionVisual> _defferedReturns = new (32);

    private void Awake()
    {
        Instance = this;

        _availableExplosions = new List<ExplosionVisual>(_poolStartSize);

        var count = 0;
        var parentTransform = transform;
        while (count < _poolStartSize)
        {
            var visual = Instantiate(_vfxPrefab, parentTransform);
            visual.gameObject.SetActive(false);
            _availableExplosions.Add(visual);
            count++;
        }
    }

    private void Update()
    {
        for (int i = _defferedReturns.Count; i --> 0;)
        {
            var vfx = _defferedReturns[i];
            if (vfx.IsFinished() == false)
                continue;

            _defferedReturns.RemoveAt(i);
            vfx.gameObject.SetActive(false);
            _availableExplosions.Add(vfx);
        }
    }

    public ExplosionVisual GetVfx()
    {
        ExplosionVisual visual = null;
        if (_availableExplosions.Count > 0)
        {
            visual = _availableExplosions[0];
            _availableExplosions.RemoveAt(0);
        }
        else
        {
            visual = Instantiate(_vfxPrefab, transform);
            visual.gameObject.SetActive(false);
        }

        return visual;
    }

    public void ReturnVfx(ExplosionVisual visual)
    {
        _defferedReturns.Add(visual);

        //visual.gameObject.SetActive(false);
        //_availableExplosions.Add(visual);
    }
}
