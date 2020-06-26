using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cone Aoe", menuName = "Card/Cone Aoe")]
public class ConeAoe : Card
{
    [Header("Cone AOE")]
    public int damage;
    public int force;
    public int range;
    public int angle;
    public float delay;


    public override void Ready()
    {
        ConeAoePlayer.Instance.Ready();
    }

    public override void Play()
    {
        ConeAoePlayer.Instance.Play();
    }
}
