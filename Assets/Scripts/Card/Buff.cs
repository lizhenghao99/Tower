using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Card/Buff")]
public class Buff : Card
{
    public enum BuffTarget {AllPlayers, AllScreen, Self};

    [Header("Buff")]
    public float duration;
    public float shieldPercent;
    [Header("Targets")]
    public BuffTarget buffTarget;
    [Header("Debuff")]
    public bool debuff;
    [Header("Apply Effect")]
    public Effect.Type effect;
    public float effectDuration;
    public float effectAmount;

    public override void Ready()
    {
        BuffPlayer.Instance.Ready();
    }

    public override void Play()
    {
        BuffPlayer.Instance.Play();
    }
}
