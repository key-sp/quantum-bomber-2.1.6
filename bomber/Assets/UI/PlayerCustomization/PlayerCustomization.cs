using System.Collections;
using System.Collections.Generic;
using Quantum;
using Quantum.Demo;
using UnityEngine;

public class PlayerCustomization : MonoBehaviour
{
	[SerializeField] private UIRoom _uiRoom = null;
	[SerializeField] private PlayerDataContainer _playerDataPrefab = null;
	
	public void OnGridSizeUpdated(string size)
	{
		Debug.Log($"Grid Size changed to {size}");
		if (byte.TryParse(size, out var convertedSize) == false) return;
		
		_uiRoom.RuntimeConfigContainer.Config.GridSize = convertedSize;
		
		Debug.Log($"Successfully updated to {convertedSize}");
	}

	public void OnBomberColorSelected(Color color)
	{
		var container = GetPlayerDataContainer();
		container.SetColor(color);
	}

	private PlayerDataContainer GetPlayerDataContainer()
	{
		var container = PlayerDataContainer.Instance == null
			? Instantiate(_playerDataPrefab)
			: PlayerDataContainer.Instance;

		return container;
	}
}
