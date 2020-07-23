using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StepEvent : MonoBehaviour
{
    public EventHandler step;

    public void OnStep()
    {
        step?.Invoke(gameObject, EventArgs.Empty);
    }
}
