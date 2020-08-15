using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenSwordsEffect : Effect
{
    protected override void OnStart()
    {
        var five = GetComponent<FiveSwordsEffect>();
        if (five != null)
        {
            five.Kill();
        }

        GetComponent<DaoshiAutoAttack>().missileCount = 7;

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
