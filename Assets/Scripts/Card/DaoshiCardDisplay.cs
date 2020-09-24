using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ProjectTower
{
    public class DaoshiCardDisplay : CardDisplay
    {
        private Image jin;
        private Image mu;
        private Image shui;
        private Image huo;
        private Image tu;

        private Image[] elements;

        protected override void DisplayResource()
        {
            primaryResource = GetComponentsInChildren<TextMeshProUGUI>()
                        .Where(c => c.gameObject.name == "PrimaryResource")
                        .FirstOrDefault();
            primaryResource.text =
                card.primaryChange < 0 ?
                (-card.primaryChange).ToString() : "0";


            jin = GetComponentsInChildren<Image>()
                        .Where(c => c.gameObject.name == "Jin")
                        .FirstOrDefault();
            mu = GetComponentsInChildren<Image>()
                        .Where(c => c.gameObject.name == "Mu")
                        .FirstOrDefault();
            shui = GetComponentsInChildren<Image>()
                        .Where(c => c.gameObject.name == "Shui")
                        .FirstOrDefault();
            huo = GetComponentsInChildren<Image>()
                        .Where(c => c.gameObject.name == "Huo")
                        .FirstOrDefault();
            tu = GetComponentsInChildren<Image>()
                        .Where(c => c.gameObject.name == "Tu")
                        .FirstOrDefault();

            elements = new Image[] { jin, mu, shui, huo, tu };
            foreach (Image i in elements)
            {
                i.enabled = false;
            }

            if (card.secondaryChange > 0)
            {
                elements[card.secondaryChange - 1].enabled = true;
            }
        }

        public override void Play()
        {
            Material mat = Instantiate(jin.material);
            jin.material = mat;

            mat = Instantiate(mu.material);
            mu.material = mat;

            mat = Instantiate(shui.material);
            shui.material = mat;

            mat = Instantiate(huo.material);
            huo.material = mat;

            mat = Instantiate(tu.material);
            tu.material = mat;

            base.Play();
        }
    }
}