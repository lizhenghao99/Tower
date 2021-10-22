using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ProjectTower;

namespace ProjectTower
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] GameObject loadingScreen;
        [SerializeField] private GameObject titleBackgroundEN;
        [SerializeField] private GameObject titleBackgroundCN;

        private void Start()
        {
            if (I2.Loc.LocalizationManager.CurrentLanguage == "Chinese")
            {
                titleBackgroundCN.SetActive(true);
            }
            else
            {
                titleBackgroundEN.SetActive(true);
            }
        }

        public void StartPlayground()
        {
            StartCoroutine(Utils.LoadAsync(2, loadingScreen));
        }

        public void StartTutorial()
        {
            StartCoroutine(Utils.LoadAsync(3, loadingScreen));
        }
        
        public void StartDeck()
        {
            StartCoroutine(Utils.LoadAsync(4, loadingScreen));
        }
        

        public void QuitGame()
        {
            Application.Quit();
        }


    }
}