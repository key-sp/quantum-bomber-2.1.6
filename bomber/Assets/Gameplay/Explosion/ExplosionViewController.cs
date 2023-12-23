using Quantum;
using UnityEngine;

public unsafe class ExplosionViewController : MonoBehaviour
{
	[SerializeField] private float _fadeOutSpeed = 5f;

	private ExplosionVisual _vfx = null;
	private Vector3 _origin = default;

	private EntityRef _entityRef = EntityRef.None;
	private float _startLifetime = float.NaN;
	private int _lastMaxReach = -1;
	private readonly int[] _lastMaxReachDirection = new int[4];

	public void OnEntityInstantiated()
	{
		_origin = transform.position;
		_entityRef = GetComponent<EntityView>().EntityRef;

		var frame = QuantumRunner.Default.Game.Frames.Predicted;
		var explosion = frame.Get<Explosion>(_entityRef);
		_startLifetime = explosion.MaxReach * explosion.CellSpreadTime.AsFloat;

		_vfx = ExplosionPool.Instance.GetVfx();
		_vfx.gameObject.SetActive(true);
		_vfx.EmitAtPosition(_origin, _startLifetime);

		SetSimulationSpeed(1f);
	}

	public void OnEntityDestroyed()
	{
		ExplosionPool.Instance.ReturnVfx(_vfx);
	}

	public void Update()
	{
		var frame = QuantumRunner.Default.Game.Frames.Predicted;
		var explosion = frame.Get<Explosion>(_entityRef);
		ExtendReach(ref explosion);

		if (explosion.HasReachedEnd == false || explosion.CurrentReach >= explosion.MaxReach)
			return;

		SetSimulationSpeed(_fadeOutSpeed);
	}

	private void ExtendReach(ref Explosion explosion)
	{
		if (explosion.CurrentReach > _lastMaxReach == false) return;

		for (var i = 0; i < 4; i++)
		{
			if (_lastMaxReachDirection[i] == explosion.ReachDirection[i]) continue;

			var direction = Direction_ext.ConvertIntToDirection(i);
			var directionVector = direction.ConvertToVector().ToUnityVector3();
			var emitPosition = _origin + directionVector * explosion.ReachDirection[i];

			_lastMaxReachDirection[i] = explosion.ReachDirection[i];

			_vfx.EmitAtPosition(emitPosition, (explosion.MaxReach - explosion.ReachDirection[i]) * 0.2f + 0.15f);
		}

		_lastMaxReach = explosion.CurrentReach;
	}

	private void SetSimulationSpeed(float simulationSpeed)
	{
		_vfx.ChangeSimulationSpeed(simulationSpeed);
	}
}
