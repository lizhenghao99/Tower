using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraPreventZFighting : MonoBehaviour {

	void Start () {
        Camera cam = gameObject.GetComponent<Camera>();
        cam.transparencySortMode = TransparencySortMode.Orthographic;

        /* Setting the TransparencySortMode to Orthographic prevents the Z Fighting that usually occurs on sprites
         * when using a Perspective Camera.
         * 
         * Another way to solve this issue is to set a particular Order in Layer for each sprite on the scene, so
         * they don't overlap by being on the same order. But changing the TransparencySortMode allows for a much
         * quicker workflow for setting up sprites, without the need to worry about the Order in Layer value
         * for every single sprite that is overlapping in the scene.
         */
    }

}
