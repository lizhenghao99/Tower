using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
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
            if (other.gameObject.name ==
                    gameObject.name.Substring(0, gameObject.name.Length - 15))
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
}