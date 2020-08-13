using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TowerUtils;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject loadingScreen;
    public static bool isPaused = false;
    private bool isSetting = false;

    private CanvasGroup group;
    private Slider loadingSlider;
    private AudioMixerManager audioMixerManager;
    private GlobalAudioManager globalAudioManager;


    private void Awake()
    {
        group = pauseMenu.GetComponent<CanvasGroup>();
        loadingSlider = loadingScreen.GetComponentInChildren<Slider>();
        audioMixerManager = AudioMixerManager.Instance;
        globalAudioManager = GlobalAudioManager.Instance;
        audioMixerManager.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                if (isSetting)
                {
                    CloseSettings();
                }
                else
                {
                    Resume();
                }
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
        group.DOFade(1f, 0.1f).SetEase(Ease.OutQuint).SetUpdate(true);
        GlobalAudioManager.Instance.Play("Pause", Vector3.zero);
        audioMixerManager.FadeOutMusic(0.5f);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        audioMixerManager.FadeInMusic(0.5f);
        group.DOFade(0f, 0.2f).SetEase(Ease.OutQuint).SetUpdate(true)
            .OnComplete(() => pauseMenu.SetActive(false));
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        audioMixerManager.FadeInMusic(0.5f);
        StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;

            yield return null;
        }
    }

    public void OpenSettings()
    {
        isSetting = true;
        group.alpha = 0f;
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        isSetting = false;
        group.alpha = 1f;
        settingsMenu.SetActive(false);
    }

    public void SetMasterVolume(float value)
    {
        audioMixerManager.SetMasterVolume(value);
    }

    public void SetBgmVolume(float value)
    {
        audioMixerManager.SetBgmVolume(value);
    }

    public void SetAmbienceVolume(float value)
    {
        audioMixerManager.SetAmbienceVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        audioMixerManager.SetSfxVolume(value);
    }

    public void LoadMainMenu()
    {
        StartCoroutine(Utils.LoadAsync(0, loadingScreen));
    }
}
