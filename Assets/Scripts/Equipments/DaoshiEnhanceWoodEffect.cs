using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class DaoshiEnhanceWoodEffect : MonoBehaviour
    {
        private GameObject player;
        // Start is called before the first frame update
        void Start()
        {
            FindObjectOfType<EffectManager>().RegisterEvent += OnRegister;
            player = GetComponentInParent<PlayerController>().gameObject;
        }

        private void OnRegister(object sender, Effect e)
        {
            if (e is WoodEffect && ((GameObject)sender) == player)
            {
                e.Start += OnWoodStart;
            }
        }

        private void OnWoodStart(object sender, EventArgs e)
        {
            WoodEffect woodEffect = (WoodEffect)sender;
            woodEffect.healPlayerPercent *= 2f;
            woodEffect.amount *= 1.5f;
            woodEffect.duration *= 1.5f;
        }
    }
}