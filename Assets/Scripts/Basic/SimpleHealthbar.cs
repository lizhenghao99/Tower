using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ProjectTower
{
    public class SimpleHealthbar : MonoBehaviour
    {
        private Image image;

        private void Start()
        {
            image = GetComponent<Image>();
        }

        public void SetValue(int value, int maxValue)
        {
            float progress = (float)value / (float)maxValue;
            image.DOFillAmount(progress, 0.3f).SetEase(Ease.OutQuint);
        }
    }
}