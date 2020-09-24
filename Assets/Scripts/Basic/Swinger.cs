using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class Swinger : MonoBehaviour
    {
        [SerializeField] float range = 45;
        [SerializeField] float speed = 1;
        [SerializeField] float offset = 0;
        [SerializeField] float decay = 1;
        [SerializeField] float frequency = 3f;
        [SerializeField] Vector3 axis = Vector3.forward;

        private float timestamp;

        // Start is called before the first frame update
        void Start()
        {
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            StartCoroutine(RepeatSwing());
        }

        // Update is called once per frame
        void Update()
        {
            float deltaT = Time.time - timestamp;
            float angle = -range * Mathf.Exp(-decay * deltaT) * Mathf.Sin(
                speed * deltaT);
            transform.localRotation = Quaternion.AngleAxis(angle, axis);
        }

        private IEnumerator RepeatSwing()
        {
            yield return new WaitForSeconds(offset + frequency);
            while (true)
            {
                StartCoroutine(SetRepeat());
                yield return new WaitForSeconds(frequency);
            }
        }

        private IEnumerator SetRepeat()
        {
            while (true)
            {
                if (transform.localRotation.eulerAngles.z < 0.5f
                    && transform.localRotation.eulerAngles.z > 0)
                {
                    timestamp = Time.time;
                    break;
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
}