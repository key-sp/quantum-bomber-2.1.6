using System.Collections;
using Quantum;
using UnityEngine;

public unsafe class GridVisualizer : MonoBehaviour
{
    [SerializeField] private Transform _groundPrefab = null;
    [SerializeField] private Transform _blockFixed = null;
    
    private Transform _levelParent = null;
    
    private IEnumerator Start()
    {
        QuantumRunner runner = null;

        do {
            yield return null;
            runner = QuantumRunner.Default;
        } while (runner == null);
        
        InitGridVisuals();
    }
    
    void InitGridVisuals()
    {
        var frame = QuantumRunner.Default.Game.Frames.Verified;

        _levelParent = this.transform;

        SetUpCamera(frame);
        SetUpGround(frame);
        SetUpCells(frame);
    }

    private void SetUpCamera(Quantum.Frame frame)
    {
        // Focus Camera

        var gridWidth = frame.Grid.GetGridWidth();
        var gridHeight = frame.Grid.GetGridHeight();
        var gridRatio = (float)gridWidth / (float)gridHeight;
        
        var cameraOffset = Vector3.zero;
        cameraOffset.x += gridWidth / 2;
        cameraOffset.y += gridHeight / 2;
        
        var localView = Camera.main;
        localView.transform.position += cameraOffset;

        if (localView.aspect >= gridRatio) {
            localView.orthographicSize = Mathf.RoundToInt(gridHeight * 0.5f);
        }
        else {
            var ratioDiff = gridRatio / localView.aspect;
            localView.orthographicSize = Mathf.RoundToInt(gridHeight * 0.5f * ratioDiff);
        }

        // To ensure the camera displays the last cell border and then some
        // purely design, no functional difference
        localView.orthographicSize += 1;
    }
    
    private void SetUpGround(Quantum.Frame frame)
    {
        var groundTransform = Instantiate(_groundPrefab, Vector3.zero, Quaternion.identity, _levelParent);

        var width = frame.Grid.GetGridWidth();
        var height = frame.Grid.GetGridHeight();
        
        var groundScale = new Vector3(width, height, 1);
        groundTransform.localScale = groundScale;
        
        var groundOffset = new Vector3(width / 2, height / 2, 1);
        groundTransform.position += groundOffset;
    }

    private void SetUpCells(Frame frame)
    {
        var width = frame.Grid.GetGridWidth();
        var height = frame.Grid.GetGridHeight();

        for (var x = 0; x < width; x++) {
            for (var y = 0; y < height; y++) {
                if (frame.Grid.GetCellPtr(x, y)->Type != CellType.BlockFix) continue;

                var blockFixed = Instantiate(_blockFixed, _levelParent);
                blockFixed.position = new Vector3(x, y);
            }
        }
    }
}
