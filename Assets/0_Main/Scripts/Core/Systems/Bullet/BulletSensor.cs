using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSensor : MonoBehaviour
{
    private BulletController _controller;

    public void Init(BulletController controller)
    {
        _controller = controller;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _controller.Target.gameObject.layer)
            _controller.OnCompleted?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _controller.Target.gameObject.layer)
            _controller.OnCompleted?.Invoke();
    }
}
