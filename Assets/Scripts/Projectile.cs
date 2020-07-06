using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public EventHandler hitFloor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            hitFloor?.Invoke(gameObject, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}
