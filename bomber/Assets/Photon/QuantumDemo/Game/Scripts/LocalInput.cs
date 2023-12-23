using System;
using Photon.Deterministic;
using Quantum;
using UnityEngine;

public class LocalInput : MonoBehaviour
{

  private const string AXIS_HORIZONTAL = "Horizontal";
  private const string AXIS_VERTICAL = "Vertical";
  private const string BUTTON_BOMB = "Jump";
  
  private void OnEnable() {
    QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
  }

  public void PollInput(CallbackPollInput callback) {
    
    var input = new Quantum.Input
    {
      MoveUp = UnityEngine.Input.GetAxis(AXIS_VERTICAL) > 0,
      MoveDown = UnityEngine.Input.GetAxis(AXIS_VERTICAL) < 0,
      MoveLeft = UnityEngine.Input.GetAxis(AXIS_HORIZONTAL) < 0,
      MoveRight = UnityEngine.Input.GetAxis(AXIS_HORIZONTAL) > 0,
      PlaceBomb = UnityEngine.Input.GetButton(BUTTON_BOMB)
    };

    callback.SetInput(input, DeterministicInputFlags.Repeatable);
  }

  private void OnDisable()
  {
    QuantumCallback.UnsubscribeListener(this);
  }
}
