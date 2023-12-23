using System;
using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;

public class PlayerDataContainer : MonoBehaviour
{
	[SerializeField] private RuntimePlayer _runtimePlayer;
	public RuntimePlayer RuntimePlayer => _runtimePlayer;
	
	public static PlayerDataContainer Instance = null;
	
	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void SetColor(Color32 color)
	{
		_runtimePlayer.Color.R = color.r;
		_runtimePlayer.Color.G = color.g;
		_runtimePlayer.Color.B = color.b;
		_runtimePlayer.Color.A = color.a;
	}

	private void OnDestroy()
	{
		Instance = null;
	}
}
