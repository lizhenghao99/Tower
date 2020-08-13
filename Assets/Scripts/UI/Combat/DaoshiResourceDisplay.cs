using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

public class DaoshiResourceDisplay : ResourceDisplay
{
    [SerializeField] float glowStart = -3.15f;
    [SerializeField] float glowEnd = 466.22f;
    [Header("YinYang")]
    [SerializeField] Image yinyangGlow;
    [SerializeField] Image yinyangShadow;
    [Header("Circle")]
    [SerializeField] Image jinCircle;
    [SerializeField] Image muCircle;
    [SerializeField] Image shuiCircle;
    [SerializeField] Image huoCircle;
    [SerializeField] Image tuCircle;
    [Header("Hanzi")]
    [SerializeField] Image jinHanzi;
    [SerializeField] Image muHanzi;
    [SerializeField] Image shuiHanzi;
    [SerializeField] Image huoHanzi;
    [SerializeField] Image tuHanzi;

    private Image[] circles;
    private Image[] hanzis;

    protected override void Start()
    {
        base.Start();
        primaryStartPercent = 0f;
        PrimaryEndPercent = 1f;
        primaryGlow.enabled = false;

        circles = 
            new Image[] { jinCircle, muCircle, shuiCircle, huoCircle, tuCircle };
        hanzis =
            new Image[] { jinHanzi, muHanzi, shuiHanzi, huoHanzi, tuHanzi };
    }

    protected override void OnResourceChanged(object sender, EventArgs e)
    {
        float percent = (float)playerResource.primaryResource /
                    (float)playerResource.maxPrimaryResource;

        primaryResource.DOFillAmount(percent, 0.3f).SetEase(Ease.OutQuint);

        if (playerResource.primaryResource < playerResource.maxPrimaryResource)
        {
            primaryGlow.enabled = true;
            primaryGlow.GetComponent<RectTransform>()
                        .DOAnchorPosX(glowStart + (glowEnd - glowStart) * percent, 0.2f)
                        .SetEase(Ease.OutQuint);
        }
        else
        {
            primaryGlow.enabled = false;
        }

        if (playerResource.secondaryResource == 0)
        {
            yinyangGlow.DOFade(1f, 0.3f).SetEase(Ease.OutQuint);
            yinyangShadow.DOFade(0f, 0.3f).SetEase(Ease.OutQuint);
        }
        else
        {
            yinyangGlow.DOFade(0f, 0.3f).SetEase(Ease.OutQuint);
            yinyangShadow.DOFade(1f, 0.3f).SetEase(Ease.OutQuint);
        }

        for (int i = 0; i < 5; i++)
        {
            if (i == playerResource.secondaryResource - 1)
            {
                circles[i].DOFade(1f, 0.3f).SetEase(Ease.OutQuint);
                hanzis[i].DOFade(1f, 0.3f).SetEase(Ease.OutQuint);
            }
            else
            {
                circles[i].DOFade(0f, 0.3f).SetEase(Ease.OutQuint);
                hanzis[i].DOFade(0f, 0.3f).SetEase(Ease.OutQuint);
            }
        }
    }
}
