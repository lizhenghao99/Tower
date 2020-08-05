using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    
    public float range;
    public EventHandler<Vector3> hitEnemy;
    public EventHandler outOfRange;
    private Vector3 startingPosition;
    private Rigidbody rigidbody;

    private void Start()
    {
        startingPosition = gameObject.transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rigidbody.velocity.magnitude != 0)
        {
            transform.rotation =
            Quaternion.LookRotation(rigidbody.velocity);
        }
        
        var currDistance =
            Vector3.Distance(gameObject.transform.position, startingPosition);
        if (currDistance > range)
        {
            outOfRange?.Invoke(gameObject, EventArgs.Empty);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            hitEnemy?.Invoke(gameObject, gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}
