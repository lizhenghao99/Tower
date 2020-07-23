using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static bool isPaused = false;

    private CanvasGroup group;


    // Start is called before the first frame update
    void Start()
    {
        group = pauseMenu.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        group.DOFade(1f, 0.3f).SetEase(Ease.OutQuint).SetUpdate(true);
        GlobalAudioManager.Instance.Play("Pause", Vector3.zero);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        group.DOFade(0f, 0.2f).SetEase(Ease.OutQuint).SetUpdate(true)
            .OnComplete(() => pauseMenu.SetActive(false));
    }

    public void Quit()
    {
        Application.Quit();
    }
}
