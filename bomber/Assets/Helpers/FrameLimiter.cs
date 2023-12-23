using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
	[SerializeField] private int _targetFrameRate = 60;
	[SerializeField] private bool _nativeRefreshRate = false;
	private void Awake()
	{
		Application.targetFrameRate = _nativeRefreshRate ? 
			Screen.currentResolution.refreshRate : _targetFrameRate;
	}
}
