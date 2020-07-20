﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEvent : MonoBehaviour
{
    public EventHandler stomp;

    public void OnStomp()
    {
        stomp?.Invoke(gameObject, EventArgs.Empty);
    }
}
