using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class HealWhenLow : MonoBehaviour
    {
        [SerializeField] float healPercent = 0.1f;
        private Health health;
        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (health.currHealth < health.maxHealth * healPercent)
            {
                health.HealPercent(1f);
            }
        }
    }
}