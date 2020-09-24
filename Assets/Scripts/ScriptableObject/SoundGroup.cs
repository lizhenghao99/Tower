using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    [CreateAssetMenu(fileName = "New Sound Group", menuName = "Sound/Sound Group")]
    public class SoundGroup : ScriptableObject
    {
        public AudioClip[] clips;
        public string soundTag;
        [Space]
        public bool loop = false;
        [Range(0, 256)]
        public int priority = 128;
        [Range(0, 1)]
        public float volume = 1;
        [Range(-3, 3)]
        public float pitch = 1;
        [Range(0, 1)]
        public float spatialBlend = 0.3f;
    }
}