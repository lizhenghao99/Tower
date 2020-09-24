using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Werewolf.StatusIndicators.Components;
using ProjectTower;
using System;

namespace ProjectTower
{
    public class AnglePlayer : Singleton<AnglePlayer>, ICardTypePlayer
    {
        private Angle cardPlaying;
        private SplatManager splat;
        private LayerMask layerMask;
        private Transform playerTransform;

        public void Awake()
        {
            layerMask = LayerMask.GetMask("Enemy");
        }

        public void Ready()
        {
            Refresh();
            splat.SelectSpellIndicator(cardPlaying.owner + "AngleSelector");
            splat.CurrentSpellIndicator.SetRange(cardPlaying.range);
            splat.CurrentSpellIndicator.Scale = cardPlaying.range * 1.2f;
            splat.CancelRangeIndicator();
        }

        public void Play()
        {
            Refresh();
            var rotation = Quaternion.LookRotation(
                    (Vector3.ProjectOnPlane(splat.Get3DMousePosition()
                    - playerTransform.position, new Vector3(0, 1, 0)).normalized));

            if (cardPlaying.vfx != null)
            {
                PlayVfx(cardPlaying, playerTransform.position, rotation);
            }

            var direction = Vector3.ProjectOnPlane(
                (splat.Get3DMousePosition() - playerTransform.position),
                new Vector3(0, 1, 0)).normalized;

            StartCoroutine(
                SpawnProjectiles(cardPlaying, playerTransform.position, direction));
        }

        private IEnumerator SpawnProjectiles(Angle c, Vector3 start, Vector3 direction)
        {
            yield return new WaitForSeconds(c.delay);

            for (int i = 0; i < c.projectileCount; i++)
            {
                var p = Instantiate(c.angleProjectile);
                p.transform.position =
                    start + new Vector3(0, cardPlaying.projectileHeight, 0);
                p.transform.rotation = Quaternion.LookRotation(direction);
                p.GetComponent<Rigidbody>()
                    .AddForce(direction * cardPlaying.projectileSpeed,
                        ForceMode.VelocityChange);
                p.SetCard(c);
                yield return new WaitForSeconds(cardPlaying.projectileInterval);
            }
        }

        private void PlayVfx(Angle c, Vector3 p, Quaternion r)
        {
            StartCoroutine(Utils.Timeout(() =>
            {
                Instantiate(
                c.vfx,
                p + r * c.vfxOffset,
                r);
            }, c.fxDelay));
        }

        private void Refresh()
        {
            cardPlaying = (Angle)CardPlayer.Instance.cardPlaying;
            splat = CardPlayer.Instance.splat;
            playerTransform = FindObjectsOfType<PlayerController>()
               .Where(player => player.gameObject.name == cardPlaying.owner.ToString())
               .FirstOrDefault().gameObject.transform;
        }
    }
}