using System;
using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;

public class BomberAnimationController : MonoBehaviour
{
	[SerializeField] private float _deathShrinkTime = 1.0f;
	[SerializeField] private AnimationCurve _animationCurve = default;
	
	// --- Need because movement animations are too slow for gameplay otherwise.
	private const float ANIMATION_SPEED = 2.0f;
	
	// --- Animation Parameters.
	private const string IS_MOVING_PARAM_NAME = "IsMoving";
	private static readonly int MOVEMENT_BOOL_ANIM_ID = Animator.StringToHash(IS_MOVING_PARAM_NAME);
	private const string IS_DEAD_PARAM_NAME = "IsDead";
	private static readonly int DEATH_BOOL_ANIM_ID = Animator.StringToHash(IS_DEAD_PARAM_NAME);

	// --- For Animation Value Evaluation
	private Animator _animator = null;
	private bool _isLocalPlayer = false;
	private EntityRef _entityRef = EntityRef.None;

	private bool _isDead = false;
	
	public void StartAnimation(EntityRef entityRef)
	{
		_entityRef = entityRef;
		_isDead = false;

		var game = QuantumRunner.Default.Game;
		var frame = game.Frames.Predicted;
		var playerRef = frame.Get<PlayerLink>(_entityRef).Id;
		_isLocalPlayer = game.PlayerIsLocal(playerRef);
		
		_animator = GetComponent<Animator>();
		_animator.speed = ANIMATION_SPEED;
		
		_animator.SetBool(DEATH_BOOL_ANIM_ID, false);
	}

	private void Update()
	{
		if (_isDead) return;
		AnimateMovement();
	}

	private void AnimateMovement()
	{
		var game = QuantumRunner.Default.Game;
		var frame = _isLocalPlayer ? game.Frames.Predicted : game.Frames.Verified;
		var movement = frame.Get<Movement>(_entityRef);

		var isMoving = movement.IsMoving;
		
		_animator.speed = isMoving ? ANIMATION_SPEED : 1.0f; 
		_animator.SetBool(MOVEMENT_BOOL_ANIM_ID, isMoving);
	}

	public float StartDeathRoutine(float delay)
	{
		_isDead = true;
		_animator.SetBool(DEATH_BOOL_ANIM_ID, true);

		StartCoroutine(DeathRoutine(delay, _deathShrinkTime));

		return delay + _deathShrinkTime;
	}

	private IEnumerator DeathRoutine(float delay, float corpseShrinkTime)
	{
		var counter = 0.0f;
		while (counter < delay)
		{
			counter += Time.deltaTime;
			yield return null;
		}

		counter = 0.0f;

		while (counter < corpseShrinkTime)
		{
			var time = counter / corpseShrinkTime;
			var modifier = _animationCurve.Evaluate(time);

			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, modifier);

			yield return null;
			counter += Time.deltaTime;
		}
	}
}
