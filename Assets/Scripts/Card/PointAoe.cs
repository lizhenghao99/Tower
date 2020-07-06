using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Point AOE", menuName = "Card/Point Aoe")]
public class PointAoe : Card
{
    [Header("Point AOE")]
    public int damage;
    public int force;
    public int range;
    public int radius;
    public float delay;
    public float fxDelay;
    [Header("Projectile")]
    public bool hasProjectile;
    public Projectile projectilePrefab;
    public float projectileHeight;
    public float projectileAngle;

    [Header("Apply Effect")]
    public Effect.Type effect;
    public float effectDuration;
    public float effectAmount;

    public override void Ready()
    {
        PointAoePlayer.Instance.Ready();
    }

    public override void Play()
    {
        PointAoePlayer.Instance.Play();
    }
}
