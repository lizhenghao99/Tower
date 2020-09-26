using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProjectTower
{
    public class StepEvent : MonoBehaviour
    {
        public event EventHandler step;

        public void OnStep()
        {
            step?.Invoke(gameObject, EventArgs.Empty);
        }
    }
}