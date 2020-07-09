using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Werewolf.StatusIndicators.Components;
using TowerUtils;
using System;

public class PointAoePlayer : Singleton<PointAoePlayer>
{
    private PointAoe cardPlaying;
    private SplatManager splat;
    private LayerMask layerMask;

    private Vector3 impactPoint;
    private PlayerController player;

    public void Awake()
    {
        layerMask = LayerMask.GetMask("Enemy");
    }

    public void Ready()
    {
        Refresh();
        splat.SelectSpellIndicator(cardPlaying.owner + "PointSelector");
        splat.CurrentSpellIndicator.SetRange(cardPlaying.range);
        splat.CurrentSpellIndicator.Scale = (cardPlaying.radius + 1) * 2;
        splat.CancelRangeIndicator();
    }

    public void Play()
    {
        Refresh();

        impactPoint = splat.GetSpellCursorPosition();



        if (cardPlaying.hasProjectile)
        {
            LaunchProjectile();
        }
        else
        {
            Impact();
        }
    }

    private void Impact()
    {
       

        var enemies = GetEnemies(
            impactPoint,
            cardPlaying.radius);

        StartCoroutine(Utils.Timeout(() =>
            Instantiate(
                cardPlaying.vfx,
                impactPoint + cardPlaying.vfxOffset,
                Quaternion.identity)
            , cardPlaying.fxDelay
        ));

        foreach (Collider e in enemies)
        {
            var direction = Vector3.ProjectOnPlane(
                e.gameObject.transform.position
                - impactPoint, new Vector3(0, 1, 0)).normalized;

            var force = direction * cardPlaying.force;
            StartCoroutine(
                hitAfterDelay(
                e.gameObject,
                cardPlaying.damage,
                force,
                cardPlaying.effect,
                cardPlaying.effectDuration,
                cardPlaying.effectAmount,
                cardPlaying.delay));
        }
    }

    private Collider[] GetEnemies(Vector3 hitPoint, float attackRange)
    {
        return Physics.OverlapSphere(hitPoint, attackRange, layerMask);
    }

    IEnumerator hitAfterDelay(GameObject e, int damage, Vector3 force,
        Effect.Type effect, float effectDuration, float effectAmount, float delay)
    {
        yield return new WaitForSeconds(delay);
        EffectManager.Instance.Register(player.gameObject, e, effect, effectDuration, effectAmount);
        e.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        e.GetComponent<Health>().TakeDamage(damage);
        yield return null;
    }

    private void Refresh()
    {
        cardPlaying = (PointAoe)CardPlayer.Instance.cardPlaying;
        splat = CardPlayer.Instance.splat;
        player = FindObjectsOfType<PlayerController>()
           .Where(player => player.gameObject.name == cardPlaying.owner.ToString())
           .FirstOrDefault();
    }

    private void LaunchProjectile()
    {
        var projectile = Instantiate(cardPlaying.projectilePrefab,
            CardPlayer.Instance.player.transform.position + 
            new Vector3 (0, cardPlaying.projectileHeight, 0),
            Quaternion.identity);
        
        projectile.GetComponent<Rigidbody>().velocity =
            Utils.getProjectileVelocity(
                projectile.transform.position,
                impactPoint,
                cardPlaying.projectileAngle);
        projectile.GetComponent<Projectile>().hitFloor 
            += OnHitFloor;
    }

    public void OnHitFloor(object sender, EventArgs e)
    {
        Impact();
    }
}
