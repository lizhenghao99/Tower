using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class WalkDust : MonoBehaviour
    {
        [SerializeField] ParticleSystem dust;
        [SerializeField] float dustDelay = 0.1f;
        [SerializeField] SpriteRenderer sprite;
        private StrideEvent stride;
        // Start is called before the first frame update
        void Start()
        {
            stride = GetComponentInChildren<StrideEvent>();
            stride.Stride += OnStride;
        }

        private void OnStride(object sender, EventArgs e)
        {
            StartCoroutine(Utils.Timeout(() =>
            {
                var d = Instantiate(dust, gameObject.transform, false);
                if (sprite.flipX)
                {
                    d.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                d.gameObject.SetActive(true);
            }, dustDelay)); 
        }
    }
}