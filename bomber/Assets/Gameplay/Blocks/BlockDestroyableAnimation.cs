using System.Collections;
using UnityEngine;

public class BlockDestroyableAnimation : MonoBehaviour
{
	[SerializeField] private AnimationCurve _animationCurve = default;
	
	private float _ttl = 0.0f;

	private Transform _transform = null;
	private Vector3 _initialSize = Vector3.zero;

	public void InitAnimation(float ttl)
	{
		_ttl = ttl;
		_transform = transform;
		_initialSize = _transform.localScale;
	}
	public IEnumerator ShrinkAnimation(float remainingTime)
	{
		var ttlCounter = _ttl - remainingTime;
		while (ttlCounter < _ttl)
		{
			var time = ttlCounter / _ttl;
			var delta = _animationCurve.Evaluate(time);
			_transform.localScale = Vector3.Lerp(_initialSize, Vector3.zero, delta);
			
			yield return null;
			
			ttlCounter += Time.deltaTime;
		}
	}
}
