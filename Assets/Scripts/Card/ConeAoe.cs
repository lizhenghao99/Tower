using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    [CreateAssetMenu(fileName = "New Cone Aoe", menuName = "Card/Cone Aoe")]
    public class ConeAoe : Card
    {
        [Header("Cone AOE")]
        public int damage;
        public float force;
        public float range;
        public float angle;
        public float delay;
        public float fxDelay;

        [Header("Apply Effect")]
        public Effect.Type effect;
        public float effectDuration;
        public float effectAmount;


        public override void Ready()
        {
            ConeAoePlayer.Instance.Ready();
        }

        public override void Play()
        {
            ConeAoePlayer.Instance.Play();
        }
    }
}