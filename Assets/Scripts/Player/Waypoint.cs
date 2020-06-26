using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public event EventHandler destinationReached;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("collision!");
        if (other.gameObject.tag == "Player")
        {
            OnDestinationReached();
            Destroy(gameObject);
        }
    }

    private void OnDestinationReached()
    {
        destinationReached?.Invoke(gameObject, EventArgs.Empty);
    }
}
