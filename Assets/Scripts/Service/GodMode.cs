using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class GodMode : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                foreach (Enemy e in FindObjectsOfType<Enemy>())
                {
                    if (!(e is PreexistEnemy))
                    {
                        e.GetComponent<Health>().TakeDamagePercent(1f);
                    }
                }
            }
        }
    }
}