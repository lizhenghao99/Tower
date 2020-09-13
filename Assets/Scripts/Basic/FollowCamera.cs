using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FollowCamera : MonoBehaviour
{
    [SerializeField] Camera target;
    [SerializeField] Camera cam;

    void Update()
    {
        cam.nearClipPlane = target.nearClipPlane;
        cam.farClipPlane = target.farClipPlane;
        cam.fieldOfView = target.fieldOfView;
    }
}
