using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public abstract class Card : ScriptableObject
{
    public enum Owner { Luban, Daoshi, thirdChar };

    [Header("Basic")]
    public string cardName;
    public bool upgraded;
    [TextArea]
    public string description;
    public int primaryChange;
    public int secondaryChange;
    public Owner owner;

    [Header("Advanced")]
    public Sprite art;
    public Sound sfx;
    public GameObject vfx;
    public Vector3 vfxOffset;
    public float castTime;
    public string animationTrigger = "Cast";
    [Header("Special")]
    public CardSpecial specialPrefab;


    public abstract void Ready();
    public abstract void Play();
}
