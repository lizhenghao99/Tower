using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    public EventHandler attack;

    public void OnAttack()
    {
        attack?.Invoke(gameObject, EventArgs.Empty);
    }
}
