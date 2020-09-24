using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class CameraCrop : MonoBehaviour
    {
        private Camera cam;
        private void Awake()
        {
            cam = GetComponent<Camera>();
            if ((float)Screen.width / Screen.height > 1.8f)
            {
                cam.rect = new Rect(0.128f, 0f, 0.744f, 1f);
            }

            if ((float)Screen.width / Screen.height < 1.6f)
            {
                cam.rect = new Rect(0f, 0.078f, 1f, 0.844f);
            }
        }
    }
}