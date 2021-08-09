using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class AttackEvent : MonoBehaviour
    {
        public event EventHandler Attack;

        public void OnAttack()
        {
            Attack?.Invoke(gameObject, EventArgs.Empty);
        }
    }
}