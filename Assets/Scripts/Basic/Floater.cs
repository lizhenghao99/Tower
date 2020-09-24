using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class Floater : MonoBehaviour
    {
        public float amplitude = 0.5f;
        public float frequency = 1f;

        Vector3 posOffset;
        Vector3 tempPos;

        void Start()
        {
            posOffset = transform.position;
            amplitude = UnityEngine.Random.Range(0, amplitude);
            frequency = UnityEngine.Random.Range(0.5f, frequency);
        }

        void Update()
        {
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = tempPos;
        }
    }
}