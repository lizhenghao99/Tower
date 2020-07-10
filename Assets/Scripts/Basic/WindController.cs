using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[ExecuteInEditMode]
public class WindController : MonoBehaviour
{
    public Material[] shortGrass;
    public Material[] highGrass;
    public Material[] leaves;

    [Range(0f, 2f)]
    public float shortGrassWindStrength;
    [Range(0f, 2f)]
    public float highGrassWindStrength;
    [Range(0f, 0.2f)]
    public float leavesWindStrength;

    private void Update()
    {
        foreach (Material m in shortGrass)
        {
            m.SetFloat("_WindStrength", shortGrassWindStrength);
        }

        foreach (Material m in highGrass)
        {
            m.SetFloat("_WindStrength", highGrassWindStrength);
        }

        foreach (Material m in leaves)
        {
            m.SetFloat("_WindStrength", leavesWindStrength);
        }
    }
}
