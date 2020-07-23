using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Card card;
    public EventHandler<Vector3> hitFloor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            hitFloor?.Invoke(gameObject, gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}
