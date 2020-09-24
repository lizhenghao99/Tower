using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class EnemyHealth : Health
    {
        public override void Die()
        {
            base.Die();
            FindObjectOfType<EnemySpawner>().CheckAllEnemiesDead();
        }
    }
}