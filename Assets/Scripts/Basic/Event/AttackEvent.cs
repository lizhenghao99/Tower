using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    public event EventHandler attack;

    public void OnAttack()
    {
        attack?.Invoke(gameObject, EventArgs.Empty);
    }
}
