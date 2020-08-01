using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using DG.Tweening;

public class AudioMixerManager : Singleton<AudioMixerManager>
{
    private AudioMixer audioMixer;
    public AudioMixerGroup bgmGroup { get; private set; }
    public AudioMixerGroup ambienceGroup { get; private set; }
    public AudioMixerGroup sfxGroup { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        audioMixer = Resources.Load<AudioMixer>("MasterMixer");
    }

    public void SetMasterVolume(float value)
    {
        var volume = Mathf.Lerp(-80, 0, value);
        audioMixer.SetFloat("Master", volume);
    }

    public void SetBgmVolume(float value)
    {
        var volume = Mathf.Lerp(-80, 0, value);
        audioMixer.SetFloat("Bgm", volume);
    }

    public void SetAmbienceVolume(float value)
    {
        var volume = Mathf.Lerp(-80, 0, value);
        audioMixer.SetFloat("Ambience", volume);
    }

    public void SetSfxVolume(float value)
    {
        var volume = Mathf.Lerp(-80, 0, value);
        audioMixer.SetFloat("Sfx", volume);
    }

    public void FadeOutMusic(float duration)
    {
        audioMixer.DOSetFloat("Lowpass", 1000f, duration)
            .SetEase(Ease.OutQuint).SetUpdate(true);
    }

    public void FadeInMusic(float duration)
    {
        audioMixer.DOSetFloat("Lowpass", 22000f, duration)
            .SetEase(Ease.OutQuint).SetUpdate(true);
    }
}
