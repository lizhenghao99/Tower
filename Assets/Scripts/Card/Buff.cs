using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    [CreateAssetMenu(fileName = "New Buff", menuName = "Card/Buff")]
    public class Buff : Card
    {
        public enum BuffTarget
        { Self, OwnMinions, AllMinions, AllChars, AllFriendlies, AllEnemies };

        [Header("Shield")]
        public float shieldPercent;
        [Header("Targets")]
        public BuffTarget buffTarget;
        [Header("Apply Effect")]
        public Effect.Type effect;
        public float effectDuration;
        public float effectAmount;

        public override void Ready()
        {
            BuffPlayer.Instance.Ready();
        }

        public override void Play()
        {
            BuffPlayer.Instance.Play();
        }
    }
}