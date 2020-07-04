using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoParallax : MonoBehaviour
{
    Camera cam;
    public BGLayer[] BGLayers;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        float camSpeed = cam.velocity.x;

        //Move the layers based on cams velocity
        if (camSpeed != 0)
            foreach (BGLayer layer in BGLayers)
            {
                float speed = camSpeed * layer.speedOverCam;

                foreach (GameObject bg in layer.bgs)
                {
                    float z = bg.transform.position.z;
                    float y = bg.transform.position.y;
                    float x = bg.transform.position.x;
                    x += speed * Time.deltaTime;
                    Vector3 newPosition = new Vector3(x, y, z);

                    bg.transform.position = newPosition;

                }
            }
    }

    [System.Serializable]
    public struct BGLayer
    {
        public GameObject[] bgs;
        [Range(-1,1)]
        public float speedOverCam;
    }

}
