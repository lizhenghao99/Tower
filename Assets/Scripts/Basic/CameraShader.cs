using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[ExecuteAlways]
public class CameraShader : MonoBehaviour
{
    public Shader shader;
    private void Start()
    {
        Camera.main.RenderWithShader(shader, "RenderType");
    }
}
