using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class IUiElement : MonoBehaviour
{
    public abstract void UpdateUi(Frame frame);
}

public class UIController : MonoBehaviour
{
    [SerializeField] private IUiElement[] _uiElements;
    
    // Update is called once per frame
    void Update()
    {
        if (QuantumRunner.Default == null) return;
        if (QuantumRunner.Default.Game == null) return;
        
        var frame = QuantumRunner.Default.Game.Frames.Predicted;

        if (frame == default) return;

        foreach (var uiElement in _uiElements) {
            uiElement.UpdateUi(frame);
        }
    }
}
