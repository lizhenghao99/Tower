using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class WorldSpaceHealthbar : MonoBehaviour
    {
        private SimpleHealthbar healthbar;
        private Health health;

        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
            healthbar = GetComponentInChildren<SimpleHealthbar>();
            health.healthChanged += OnHealthChanged;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnHealthChanged(object sender, EventArgs e)
        {
            healthbar.SetValue(health.currHealth, health.maxHealth);
        }
    }
}