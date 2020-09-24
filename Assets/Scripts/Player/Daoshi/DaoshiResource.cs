using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ProjectTower
{
    public class DaoshiResource : PlayerResource
    {
        [SerializeField] int autoPrimaryRegen = 2;

        private LevelController levelController;
        private PlayerHealth health;
        private Coroutine autogen;

        protected override void Start()
        {
            base.Start();
            levelController = FindObjectOfType<LevelController>();
            levelController.StartCombat += OnStartAutogen;
            levelController.EndCombat += OnEndAutogen;
            health = GetComponent<PlayerHealth>();
            health.death += OnEndAutogen;
            health.revive += OnStartAutogen;
        }

        public override bool IsResourceEnough(int primaryAmount, int secondaryAmount)
        {
            var primary = primaryResource + primaryAmount;

            return primary >= 0;
        }

        public override bool ChangeResource(int primaryAmount, int secondaryAmount)
        {
            if (IsResourceEnough(primaryAmount, secondaryAmount))
            {
                primaryResource = Mathf.Clamp(
                    primaryResource + primaryAmount, 0, 100);
                if (secondaryAmount != -1)
                {
                    secondaryResource = secondaryAmount;
                }
                OnResourceChanged();
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator ResourceAutoGen()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                if (secondaryResource == 0)
                {
                    ChangeResource(autoPrimaryRegen * 2, -1);
                }
                else
                {
                    ChangeResource(autoPrimaryRegen, -1);
                }
            }
        }

        private void OnResourceChanged()
        {
            InvokeResourceChanged(gameObject);
        }

        private void OnStartAutogen(object sender, EventArgs e)
        {
            autogen = StartCoroutine(ResourceAutoGen());
        }

        private void OnEndAutogen(object sender, EventArgs e)
        {
            StopCoroutine(autogen);
        }
    }
}