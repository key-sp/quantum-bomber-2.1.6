using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAnimationControllerMenu : MonoBehaviour
{
	private const string WAVE_PARAM_NAME = "Wave";
	private static readonly int WAVE_TRIGGER_ANIM_ID = Animator.StringToHash(WAVE_PARAM_NAME);

	[SerializeField] private Animator _animator = null;

	public void ConfirmSelectionReaction(Color color)
	{
		_animator.SetTrigger(WAVE_TRIGGER_ANIM_ID);
	}
}
