using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[ExecuteInEditMode]
public class WindController : MonoBehaviour
{
    public Material[] materials;

    [Range(0f, 2f)]
    public float windStrength;

    private void Update()
    {
        foreach (Material m in materials)
        {
            m.SetFloat("_WindStrength", windStrength);
        }
    }
}
