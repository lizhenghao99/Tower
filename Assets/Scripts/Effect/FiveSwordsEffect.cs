using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
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
            currentVfx.transform.localPosition = originalPos;
            currentVfx.transform.localScale = originalScale;
        }

        protected override void OnFinish()
        {
            GetComponent<DaoshiAutoAttack>().missileCount = 3;
            base.OnFinish();
        }
    }
}