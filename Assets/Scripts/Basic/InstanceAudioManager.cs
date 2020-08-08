using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Audio;

public class InstanceAudioManager : MonoBehaviour
{
    [SerializeField] SoundGroup[] soundGroups;
    private AudioSource audioSource;
    private StepEvent step;
    private StrideEvent stride;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        step = GetComponentInChildren<StepEvent>();
        if (step != null)
        {
            step.step += OnStep;
        }

        stride = GetComponentInChildren<StrideEvent>();
        if (stride != null)
        {
            stride.stride += OnStride;
        }

        audioSource.outputAudioMixerGroup = AudioMixerManager.Instance.sfxGroup;

        Play("Spawn");
        Play("SpecialSpawn");
    }

    private void OnStep(object sender, EventArgs e)
    {
        Play("Step");
    }

    private void OnStride(object sender, EventArgs e)
    {
        Play("Stride");
    }

    public void Play(string tag)
    {
        if (soundGroups == null) return;
        SoundGroup desiredGroup =
            soundGroups.Where(s => s.soundTag == tag).FirstOrDefault();

        if (desiredGroup == null) return;

        audioSource.loop = desiredGroup.loop;
        audioSource.priority = desiredGroup.priority;
        audioSource.pitch = desiredGroup.pitch;
        audioSource.spatialBlend = desiredGroup.spatialBlend;

        PlaySound(desiredGroup);

    }

    private void PlaySound(SoundGroup desiredGroup)
    {
        AudioClip[] desiredClips = desiredGroup.clips;
        if (desiredClips != null && desiredClips.Length != 0)
        {
            if (desiredClips.Length == 1)
            {

                audioSource.PlayOneShot(desiredClips[0], desiredGroup.volume);
            }
            else
            {
                int index = UnityEngine.Random.Range(0, desiredClips.Length);
                audioSource.PlayOneShot(desiredClips[index], desiredGroup.volume);
            }
        }
    }
}
