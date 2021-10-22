using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ProjectTower
{
    public class LanguageMenu : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;

        private void Awake()
        {
            var gam = GlobalAudioManager.Instance;
        }

        public void CloseMenu()
        {
            StartCoroutine(Utils.LoadAsync(1, loadingScreen));
        }
    }
}

