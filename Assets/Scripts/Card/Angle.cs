using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    [CreateAssetMenu(fileName = "New Angle", menuName = "Card/Angle")]
    public class Angle : Card
    {
        [Header("Angle")]
        public int damage;
        public float force;
        public float range;
        public float radius;
        public float delay;
        public float fxDelay;

        [Header("Projectile")]
        public AngleProjectile angleProjectile;
        public float projectileSpeed = 5f;
        public float projectileHeight = 2f;
        public int projectileCount = 1;
        public float projectileInterval = 1f;

        [Header("Apply Effect")]
        public Effect.Type effect;
        public float effectDuration;
        public float effectAmount;

        public override void Ready()
        {
            AnglePlayer.Instance.Ready();
        }

        public override void Play()
        {
            AnglePlayer.Instance.Play();
        }
    }
}