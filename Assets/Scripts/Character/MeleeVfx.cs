using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class MeleeVfx : MonoBehaviour
    {
        [SerializeField] GameObject vfx;
        [SerializeField] float delay;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<AttackEvent>().Attack += OnAttack;
        }

        private void OnAttack(object sender, EventArgs e)
        {
            var fx = Instantiate(vfx);
            fx.transform.position = gameObject.transform.position;
        }
    }
}