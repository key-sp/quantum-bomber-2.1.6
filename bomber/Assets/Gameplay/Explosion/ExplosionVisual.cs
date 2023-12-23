using Quantum;
using UnityEngine;

public class ExplosionVisual : MonoBehaviour
{
	private const int PARTICLE_BUFFER_SIZE = 100;

	[SerializeField] private ParticleSystem[] _vfx = null;
	[SerializeField] private int _emitCount = 10;

	private Vector3 _lastPosition = Vector3.negativeInfinity;

	private readonly ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[PARTICLE_BUFFER_SIZE];

	public void ChangeSimulationSpeed(float simSpeed)
	{
		foreach (var vfx in _vfx)
		{
			var main = vfx.main;
			main.simulationSpeed = simSpeed;
		}
	}

	public void EmitAtPosition(Vector3 position, float startLifetime)
	{
		if (_lastPosition.Equals(position)) return;

		_lastPosition = position;

		var parameters = new ParticleSystem.EmitParams {
			position = position,
			startLifetime = startLifetime,
			applyShapeToPosition = true,
		};

		foreach (var vfx in _vfx) {
			vfx.Emit(parameters, _emitCount);
		}
	}

	public bool IsFinished()
	{
		for (int i = 0; i < _vfx.Length; i++)
		{
			_vfx[i].GetParticles(_particles);

			if (_vfx[i].particleCount > 0)
				return false;
		}

		return true;
	}
}
