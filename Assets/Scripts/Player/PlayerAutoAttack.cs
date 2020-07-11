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
        
        splat.SelectRangeIndicator(gameObject.name + "Range");
        splat.CurrentRangeIndicator.DefaultScale = (attackRange + 1) * 2;
        splat.CurrentRangeIndicator.Scale = (attackRange + 1) * 2;
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetEnemies(gameObject.transform.position, attackRange);
        SetTarget();
        if (!player.isWalking && !player.isCasting)
        {
            ApplyTaunt();
            Attack();
        }
    }

    protected override bool TauntedBehavior()
    {
        return false;
    }
}
