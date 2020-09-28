using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class PreexistEnemy : Enemy
    {
        private AttackBase attack;
        private Collider collider;

        protected override void Awake()
        {
            // do nothing
        }

        protected override void Start()
        {
            attack = GetComponent<AttackBase>();
            collider = GetComponent<Collider>();
            attack.enabled = false;
            //collider.enabled = false;
        }

        public override void Spawn()
        {
            attack.enabled = true;
           // collider.enabled = true;
        }
    }
}