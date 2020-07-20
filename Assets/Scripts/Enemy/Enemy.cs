using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    protected virtual void Awake()
    {
        gameObject.SetActive(false);
    }

    protected virtual void Start()
    {
        // do nothing
    }

    public virtual void Spawn()
    {
        gameObject.SetActive(true);
    }  

}
