using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ProjectTower
{
    public class Healthbar : MonoBehaviour
    {
        public PlayerController player;
        [SerializeField] float glowStart = -135f;
        [SerializeField] float glowEnd = 132.5f;

        private PlayerHealth playerHealth;
        private Image healthbar;
        private Image healthGlow;
        private Image shieldbar;
        private Image shieldGlow;


        // Start is called before the first frame update
        void Start()
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            healthbar = GetComponentsInChildren<Image>()
                            .Where(i => i.gameObject.name == "HealthbarFill")
                            .FirstOrDefault();
            shieldbar = GetComponentsInChildren<Image>()
                            .Where(i => i.gameObject.name == "HealthbarShield")
                            .FirstOrDefault();
            healthGlow = GetComponentsInChildren<Image>()
                           .Where(i => i.gameObject.name == "HealthbarHealthGlow")
                           .FirstOrDefault();
            shieldGlow = GetComponentsInChildren<Image>()
                           .Where(i => i.gameObject.name == "HealthbarShieldGlow")
                           .FirstOrDefault();

            playerHealth.healthChanged += OnHealthChanged;
            playerHealth.shieldChanged += OnShieldChanged;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnHealthChanged(object sender, EventArgs e)
        {
            float percent = (float)playerHealth.currHealth / (float)playerHealth.maxHealth;
            healthbar.DOFillAmount(percent, 0.2f).SetEase(Ease.OutQuint);

            if (playerHealth.currHealth < playerHealth.maxHealth)
            {
                healthGlow.enabled = true;
                healthGlow.GetComponent<RectTransform>()
                            .DOAnchorPosX(glowStart + (glowEnd - glowStart) * percent, 0.2f)
                            .SetEase(Ease.OutQuint);
            }
            else
            {
                healthGlow.enabled = false;
            }
        }

        private void OnShieldChanged(object sender, EventArgs e)
        {
            float percent = (float)playerHealth.currShield / (float)playerHealth.maxShield;
            shieldbar.DOFillAmount(percent, 0.2f).SetEase(Ease.OutQuint);

            if (playerHealth.currShield > 0)
            {
                shieldGlow.enabled = true;
                shieldGlow.GetComponent<RectTransform>()
                            .DOAnchorPosX(glowStart + (glowEnd - glowStart) * percent, 0.2f)
                            .SetEase(Ease.OutQuint);
            }
            else
            {
                shieldGlow.enabled = false;
            }
        }
    }
}