using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Werewolf.StatusIndicators.Components;
using TowerUtils;
using System;

public class PointAoePlayer : Singleton<PointAoePlayer>, ICardTypePlayer
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
            Impact(cardPlaying, impactPoint);
        }
    }

    private void Impact(PointAoe c, Vector3 p)
    {
        GlobalAudioManager.Instance.Play(c.impactSfx, p);

        var enemies = GetEnemies(
            p,
            c.radius);

        StartCoroutine(Utils.Timeout(() => {
            if (c.vfx != null)
            {
                var fx = Instantiate(c.vfx);
                fx.transform.position = p + c.vfxOffset;
            }
        }, c.fxDelay));

        foreach (Collider e in enemies)
        {
            var direction = Vector3.ProjectOnPlane(
                e.gameObject.transform.position
                - p, new Vector3(0, 1, 0)).normalized;

            var force = direction * c.force;
            StartCoroutine(
                hitAfterDelay(
                e.gameObject,
                c.damage,
                force,
                c.effect,
                c.effectDuration,
                c.effectAmount,
                c.delay));
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
        FindObjectOfType<EffectManager>().Register(player.gameObject, e, effect, effectDuration, effectAmount);
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

        var axis = Quaternion.Euler(30, 0, 0) * Vector3.back;

        var angularVelocity = 1.5f * (float)Math.PI;

        if (impactPoint.x < projectile.transform.position.x)
        {
            angularVelocity *= -1;
        }

        projectile.GetComponent<Rigidbody>().angularVelocity =
            axis * angularVelocity;

        projectile.GetComponent<PointProjectile>().hitFloor 
            += OnHitFloor;

        projectile.GetComponent<PointProjectile>().card = cardPlaying;
    }

    public void OnHitFloor(object sender, Vector3 point)
    {
        GameObject projectile = (GameObject)sender;
        
        Impact((PointAoe)projectile.GetComponent<PointProjectile>().card, point);
    }
}
