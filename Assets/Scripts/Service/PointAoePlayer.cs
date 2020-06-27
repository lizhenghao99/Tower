using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Werewolf.StatusIndicators.Components;

public class PointAoePlayer : Singleton<PointAoePlayer>
{
    private PointAoe cardPlaying;
    private SplatManager splat;
    private LayerMask layerMask;

    public void Awake()
    {
        cardPlaying = (PointAoe)CardPlayer.Instance.cardPlaying;
        splat = CardPlayer.Instance.splat;
        layerMask = LayerMask.GetMask("Enemy");
    }

    public void Ready()
    {
        splat.SelectSpellIndicator(cardPlaying.owner + "PointSelector");
        splat.CurrentSpellIndicator.SetRange(cardPlaying.range);
        splat.CurrentSpellIndicator.Scale = cardPlaying.radius * 2;
        splat.CancelRangeIndicator();
    }

    public void Play()
    {
        Instantiate(
            cardPlaying.vfx,
            splat.GetSpellCursorPosition() + cardPlaying.vfxOffset,
            Quaternion.identity);

        var enemies = GetEnemies(
            splat.GetSpellCursorPosition(),
            cardPlaying.radius);

        foreach(Collider e in enemies)
        {
            var direction = Vector3.ProjectOnPlane(
                e.gameObject.transform.position
                - splat.GetSpellCursorPosition(), new Vector3(0,1,0)).normalized;

            var force = direction * cardPlaying.force;
            StartCoroutine(
                hitAfterDelay(
                e.gameObject,
                cardPlaying.damage,
                force,
                cardPlaying.delay));
        }

    }

    private Collider[] GetEnemies(Vector3 hitPoint, float attackRange)
    {
        return Physics.OverlapSphere(hitPoint, attackRange, layerMask);
    }

    IEnumerator hitAfterDelay(GameObject e, int damage, Vector3 force, float delay)
    {
        yield return new WaitForSeconds(delay);
        e.GetComponent<Health>().TakeDamage(damage);
        e.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        yield return null;
    }
}
