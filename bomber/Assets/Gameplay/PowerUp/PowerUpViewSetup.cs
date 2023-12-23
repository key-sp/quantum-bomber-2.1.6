using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpViewSetup : MonoBehaviour
{

    [SerializeField] private GameObject _pickedUpVfx = null;
    [SerializeField] private float _pickedUpVfxDestroyDelay = 1.0f;

    public void OnEntityDestroyed()
    {
        var vfx = Instantiate(_pickedUpVfx, transform.position, Quaternion.identity);
        Destroy(vfx, _pickedUpVfxDestroyDelay);
    }

}
