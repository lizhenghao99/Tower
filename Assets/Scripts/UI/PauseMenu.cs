using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static bool isPaused = false;

    private CanvasGroup group;
    private AudioLowPassFilter lowPassFilter;


    // Start is called before the first frame update
    void Start()
    {
        group = pauseMenu.GetComponent<CanvasGroup>();
        lowPassFilter = Camera.main.GetComponentInChildren<AudioLowPassFilter>();
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
        DOTween.To(() => lowPassFilter.cutoffFrequency,
            (x) => lowPassFilter.cutoffFrequency = x,
            2000f, 0.5f).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        DOTween.To(() => lowPassFilter.cutoffFrequency,
            (x) => lowPassFilter.cutoffFrequency = x,
            22000f, 0.5f).SetEase(Ease.OutQuint).SetUpdate(true);
        group.DOFade(0f, 0.2f).SetEase(Ease.OutQuint).SetUpdate(true)
            .OnComplete(() => pauseMenu.SetActive(false));
    }

    public void Quit()
    {
        Application.Quit();
    }
}
