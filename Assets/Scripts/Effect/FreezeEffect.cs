using System.Collections;
using System.Collections.Generic;

using UnityEngine.AI;

namespace ProjectTower
{
    public class FreezeEffect : Effect
    {
        private NavMeshAgent agent;
        private float originalSpeed;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        protected override void OnStart()
        {
            var mu = GetComponent<WoodEffect>();
            if (mu != null)
            {
                mu.Enhance();
            }

            var huo = GetComponent<BurnEffect>();
            if (huo != null)
            {
                huo.Kill();
                OnFinish();
                return;
            }

            originalSpeed = agent.speed;
            agent.speed *= (1 - amount);
            base.OnStart();
        }

        protected override void OnFinish()
        {
            agent.speed = originalSpeed;
            base.OnFinish();
        }
    }
}