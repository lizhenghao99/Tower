using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Level/Stage")]
public class Stage : ScriptableObject
{
    public int index;
    [Space]
    public Vector3 basePosition;
    public Vector3[] charPostions;
    [Header("Camera Bounds")]
    public float top;
    public float bottom;
    public float left;
    public float right;
    [Header("Environment")]
    public float sunLightIntensity = 1f;
}