using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Point AOE", menuName = "Card/Point Aoe")]
public class PointAoe : Card
{
    [Header("Point AOE")]
    public int damage;
    public float force;
    public float range;
    public float radius;
    public float delay;
    public float fxDelay;
    [Header("Projectile")]
    public bool hasProjectile;
    public Projectile projectilePrefab;
    public Sound impactSfx;
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
