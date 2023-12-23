using Quantum;
using UnityEngine;

public class BombViewSetup : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _explosionVFX = null;
    [SerializeField] private float _vfxTTL = 1.0f;

    public void OnEntityDestroyed()
    {
        if (QuantumRunner.Default == null) return;
        
        var index = Random.Range(0, _explosionVFX.Length);
        var explosion = Instantiate(_explosionVFX[index], transform.position, transform.rotation).gameObject;
        Destroy(explosion, _vfxTTL);
    }
}
