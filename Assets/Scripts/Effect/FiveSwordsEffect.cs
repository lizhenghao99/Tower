using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveSwordsEffect : Effect
{
    protected override void OnStart()
    {
        var seven = GetComponent<SevenSwordsEffect>();
        if (seven)
        {
            seven.Kill();
        }

        GetComponent<DaoshiAutoAttack>().missileCount = 5;

        base.OnStart();
        currentVfx.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        currentVfx.transform.localScale = Vector3.one * 1f;
    }

    protected override void OnFinish()
    {
        GetComponent<DaoshiAutoAttack>().missileCount = 3;
        base.OnFinish();
    }
}
