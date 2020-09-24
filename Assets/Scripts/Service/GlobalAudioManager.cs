using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectTower
{
    public class GlobalAudioManager : Singleton<GlobalAudioManager>
    {
        public Sound[] sounds;
        private AudioMixerManager audioMixerManager;

        private void Start()
        {
            sounds = Resources.LoadAll<Sound>("Sounds");
            audioMixerManager = AudioMixerManager.Instance;
        }

        public void Play(Sound sound, Vector3 position)
        {
            if (sound != null)
            {
                PlayAtLocation(sound, position);
            }
        }

        public void Play(string tag, Vector3 position)
        {
            Sound[] desiredSounds = sounds.Where(s => s.soundTag == tag).ToArray();

            if (desiredSounds != null && desiredSounds.Length != 0)
            {
                if (desiredSounds.Length == 1)
                {

                    PlayAtLocation(desiredSounds[0], position);
                }
                else
                {
                    int index = Random.Range(0, desiredSounds.Length);
                    PlayAtLocation(desiredSounds[index], position);
                }
            }
        }

        private void PlayAtLocation(Sound sound, Vector3 position)
        {
            var tempGO = new GameObject("TempAudio");
            tempGO.transform.position = position;
            var audioSource = tempGO.AddComponent<AudioSource>();

            audioSource.outputAudioMixerGroup = audioMixerManager.sfxGroup;

            audioSource.clip = sound.clip;
            audioSource.loop = sound.loop;
            audioSource.priority = sound.priority;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.spatialBlend = sound.spatialBlend;

            audioSource.Play();
            Destroy(tempGO, sound.clip.length);
        }
    }
}