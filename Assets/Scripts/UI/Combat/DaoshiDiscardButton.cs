using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TowerUtils;
using DG.Tweening;

public class DaoshiDiscardButton : DiscardButton
{
    private CanvasGroup discardHighlight;

    protected override void Start()
    {
        base.Start();
        discardHighlight = GetComponentsInChildren<CanvasGroup>()
            .Where(g => g.gameObject.name == "DiscardHighlight")
            .FirstOrDefault();
        discardHighlight.alpha = 0f;
    }

    protected override void PointerEnterBehavior()
    {
        // do nothing
    }

    protected override void PointerExitBehavior()
    {
        // do nothing
    }

    protected override void PointerDownBehavior()
    {
        discardHighlight.DOFade(1f, 0.3f).SetEase(Ease.OutQuint);
        GlobalAudioManager.Instance.Play("Meditate", Vector3.zero);
    }

    protected override void DeselecteBehavior()
    {
        discardHighlight.DOFade(0f, 0.3f).SetEase(Ease.OutQuint);
        DoStateTransition(SelectionState.Normal, false);
    }

    protected override bool DiscardBehavior(Card card)
    {
        player.setIsCasting(true);
        player.GetComponent<DaoshiResource>()
                    .ChangeResource(card.primaryChange / 2, -1);
        StartCoroutine(Utils.Timeout(() =>
            player.setIsCasting(false), 1.5f));

        return true;
    }
}
