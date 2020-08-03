using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
using System;

public class MenuButton : Selectable
{
    [Serializable]
    public class ClickEvent : UnityEvent { }

    public ClickEvent onClick;
    private Image underline;
    private float animationTime = 0.1f;

    protected override void Awake()
    {
        base.Awake();
        underline = GetComponentsInChildren<Image>()
            .Where(i => i.name == "Underline").FirstOrDefault();
        underline.fillAmount = 0f;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        underline.DOFillAmount(1f, animationTime)
            .SetEase(Ease.OutQuint).SetUpdate(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        underline.DOFillAmount(0f, animationTime)
            .SetEase(Ease.OutQuint).SetUpdate(true);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        underline.fillAmount = 0f;
        onClick.Invoke(); 
    }
}
