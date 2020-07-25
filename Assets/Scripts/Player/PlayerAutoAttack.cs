using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Werewolf.StatusIndicators.Components;

public class PlayerAutoAttack : AttackBase
{
    public SplatManager splat { get; set; }
    private PlayerController player;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        layerMask = LayerMask.GetMask("Enemy");

        player = GetComponent<PlayerController>();
        splat = GetComponentInChildren<SplatManager>();

        LevelController.Instance.StartCombat += OnStartCombat;
        LevelController.Instance.EndCombat += OnEndCombat;
    }

    public void OnStartCombat(object sender, EventArgs e)
    {
        splat.SelectRangeIndicator(gameObject.name + "Range");
        splat.CurrentRangeIndicator.DefaultScale = (attackRange + 1) * 2;
        splat.CurrentRangeIndicator.Scale = (attackRange + 1) * 2;
    }

    public void OnEndCombat(object sender, EventArgs e)
    {
        splat.CancelRangeIndicator();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (health.isDead)
        {
            agent.destination = gameObject.transform.position;
            agent.isStopped = true;
            return;
        }
        GetEnemies(gameObject.transform.position, attackRange);
        SetTarget();
        if (!player.isWalking)
        {
            ApplyTaunt();
            if (!player.isCasting)
            {
                Attack();
            }
        }
    }

    protected override bool TauntedBehavior()
    {
        return false;
    }

    protected override void NoTargetBehavior()
    {
        // do nothing
    }
}
