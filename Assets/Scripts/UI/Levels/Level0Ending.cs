using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Level0Ending : LevelEnding
{
    private EliteWolfAttack boss;
    private Health bossHealth;

    protected override void Start()
    {
        base.Start();
        boss = FindObjectOfType<EliteWolfAttack>();
        bossHealth = boss.GetComponent<Health>();
    }

    public override void Lose()
    {
        if (levelController.currStage.index == 2 &&
            (float)bossHealth.currHealth <= (int)(bossHealth.maxHealth * 0.1))
        {
            base.Win();
        }
        else
        {
            if (FindObjectOfType<TutorialController>() == null)
            {
                base.Lose();
            }
        }
    }
}
