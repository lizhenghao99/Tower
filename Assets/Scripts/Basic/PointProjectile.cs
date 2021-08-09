using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class PointProjectile : MonoBehaviour
    {
        public Card card;
        public event EventHandler<Vector3> HitFloor;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Floor")
            {
                HitFloor?.Invoke(gameObject, gameObject.transform.position);
                Destroy(gameObject);
            }
        }
    }
}