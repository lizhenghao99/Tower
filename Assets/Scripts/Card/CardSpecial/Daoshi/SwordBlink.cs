using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SwordBlink : CardSpecial
{
    [SerializeField] GameObject startFx;
    [SerializeField] GameObject endFx;

    private GameObject player;

    private void Start()
    {
        var colliders = Physics.OverlapSphere(gameObject.transform.position, 0.1f);

        Collider nearestCollider = null;
        float nearestDistance = float.MaxValue;
        float distance;

        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Floor"))
            {
                continue;
            }
            distance =
                Vector3.Distance(player.transform.position, c.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestCollider = c;
            }
        }

        if (nearestCollider != null)
        {
            var direction = Vector3.ProjectOnPlane(
                nearestCollider.transform.position - player.transform.position, new Vector3(0, 1, 0)).normalized;
            RaycastHit[] hits;
            hits = Physics
                .RaycastAll(player.transform.position,
                            direction);

            if (hits.Any(h => h.collider == nearestCollider))
            {
                var hitInfo = hits.Where(h => h.collider == nearestCollider)
                 .FirstOrDefault();

                var closest = Vector3.ProjectOnPlane(hitInfo.point, 
                    new Vector3(0,1,0));

                gameObject.transform.position = closest - direction * 2f;
            }
        }
    }

    protected override void SetLifetime()
    {
        lifetime = 2f;
    }

    protected override void OnStart()
    {
        player = FindObjectOfType<DaoshiAutoAttack>().gameObject;
        Instantiate(startFx, player.transform);

        base.OnStart();
    }

    protected override void SelfDestroy()
    {
        var agent = player.GetComponent<NavMeshAgent>();
        var collider = player.GetComponent<Collider>();
        var controller = player.GetComponent<PlayerController>();

        Instantiate(endFx, player.transform);
        agent.enabled = false;
        collider.enabled = false;
        if (controller.isWalking)
        {
            Destroy(controller.myWaypoint.gameObject);
            controller.myWaypoint = null;
            controller.OnDestinationReached(gameObject, EventArgs.Empty);
        }

        player.transform.position = gameObject.transform.position;

        collider.enabled = true;
        agent.enabled = true;
        agent.SetDestination(player.transform.position);
        base.SelfDestroy();
    }
}
