using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Summon", menuName = "Card/Summon")]
public class Summon : Card
{
    [Header("Summon")]
    public float range;
    public float radius;
    [Header("Minon")]
    public Minion minionPrefab;
    public MinionCenter centerPrefab;
    public int count;
    public int attackDamage;
    public float attackRange;
    public float attackRate;
    public float stopRange;
    [Header("Minion Type")]
    public bool taunt;
    public bool invisible;
    public bool guard;
    public bool charge;
    public bool ambush;

    public override void Ready()
    {
        SummonPlayer.Instance.Ready();
    }

    public override void Play()
    {
        SummonPlayer.Instance.Play();
    }
}

