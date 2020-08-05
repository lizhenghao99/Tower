using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "Sound/Sound")]
public class Sound : ScriptableObject
{
    public AudioClip clip;
    public string soundTag;
    [Space]
    public bool loop = false;
    [Range(0, 256)]
    public int priority = 128;
    [Range(0,1)]
    public float volume = 0.7f;
    [Range(-3, 3)]
    public float pitch = 1;
    [Range(0, 1)]
    public float spatialBlend = 0.3f;
}
