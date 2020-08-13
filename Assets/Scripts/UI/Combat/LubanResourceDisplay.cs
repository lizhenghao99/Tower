using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

public class LubanResourceDisplay : ResourceDisplay
{
    protected override void OnResourceChanged(object sender, EventArgs e)
    {
        float percent = (float)playerResource.primaryResource /
                    (float)playerResource.maxPrimaryResource;
        float amount = primaryStartPercent
            + (PrimaryEndPercent - primaryStartPercent) * percent;
        primaryResource.DOFillAmount(amount, 0.2f)
                       .SetEase(Ease.OutQuint);

        float glowStart = 90f - 360 * primaryStartPercent;
        float glowRotatoin = glowStart - 360 * (amount - primaryStartPercent);
        primaryGlow.transform.DORotate(
            new Vector3(0, 0, glowRotatoin), 0.2f)
            .SetEase(Ease.OutQuint);

        Image[] secondaryResource = new Image[3];
        for (int i = 0; i < 3; i++)
        {
            secondaryResource[i] = GetComponentsInChildren<Image>()
            .Where(image => image.gameObject.name
                == $"SecondaryDisplay{i + 1}Fill")
            .FirstOrDefault();
            if (i < playerResource.secondaryResource)
            {
                secondaryResource[i].DOFade(1f, 0.2f).SetEase(Ease.OutQuint);
            }
            else
            {
                secondaryResource[i].DOFade(0f, 0.2f).SetEase(Ease.OutQuint);
            }
        }
    }
}
