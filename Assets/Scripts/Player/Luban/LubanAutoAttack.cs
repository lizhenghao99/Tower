using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LubanAutoAttack : PlayerAutoAttack
{
    protected override void SpecialAutoAttack()
    {
        GetComponent<LubanResource>().ResourceAutoGen();
    }
}
