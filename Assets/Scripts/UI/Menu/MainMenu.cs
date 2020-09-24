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

        public void StartGame()
        {
            StartCoroutine(Utils.LoadAsync(1, loadingScreen));
        }

        public void QuitGame()
        {
            Application.Quit();
        }


    }
}