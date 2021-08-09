using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class StompEvent : MonoBehaviour
    {
        public event EventHandler Stomp;

        public void OnStomp()
        {
            Stomp?.Invoke(gameObject, EventArgs.Empty);
        }
    }
}