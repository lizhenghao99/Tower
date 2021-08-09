using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProjectTower
{
    public class StrideEvent : MonoBehaviour
    {
        public event EventHandler Stride;

        public void OnStride()
        {
            Stride?.Invoke(gameObject, EventArgs.Empty);
        }
    }
}