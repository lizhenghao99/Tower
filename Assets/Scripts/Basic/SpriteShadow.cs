using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    [ExecuteAlways]
    public class SpriteShadow : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            foreach (Renderer r in GetComponentsInChildren<SpriteRenderer>())
            {
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }

            var renderer = GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.shadowCastingMode =
                    UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }

        private void OnDisable()
        {
            foreach (Renderer r in GetComponentsInChildren<SpriteRenderer>())
            {
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            var renderer = GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.shadowCastingMode =
                    UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }
    }
}