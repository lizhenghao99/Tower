using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProjectTower
{
    public class StepEvent : MonoBehaviour
    {
        public event EventHandler Step;

        public void OnStep()
        {
            Step?.Invoke(gameObject, EventArgs.Empty);
        }
    }
}