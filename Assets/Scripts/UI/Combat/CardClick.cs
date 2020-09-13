using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Linq;
using TowerUtils;

public class CardClick : Selectable
{
    public Card card { get; private set; }

    public float xOffset;
    public float xSpacing;
    public float cardYPos;
    public float cardWidth;
    public float cardHeight;
    public float zoomFactor;

    public Material cardMaterial;
    public CanvasGroup group;

    private RectTransform rectTransform;
    private HandManager handManager;
    private DiscardButton discardButton;

    private float fadeTime = 0.3f;

    private List<CardClick> hand;

    private InspectMenu inspectMenu;

    private CardDisplay display;

    protected override void Awake()
    {
        group = GetComponent<CanvasGroup>();
        display = GetComponent<CardDisplay>();
    }

    protected override void Start()
    {
        base.Start();
        rectTransform = GetComponent<RectTransform>();
        handManager = GetComponentInParent<HandManager>();
        discardButton = gameObject.transform.parent.parent.GetComponentInChildren<DiscardButton>();
        inspectMenu = FindObjectOfType<InspectMenu>();
    }

    public void SetCard(Card c)
    {
        card = c;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (IsInteractable()
            && EventSystem.current.currentSelectedGameObject != gameObject)
        {
            ZoomIn();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (IsInteractable() 
            && EventSystem.current.currentSelectedGameObject != gameObject)
        {
            rectTransform.DOSizeDelta(
                new Vector2(cardWidth, cardHeight), 0.3f)
                .SetEase(Ease.OutQuint);
            ZoomOut();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (IsInteractable())
        {
            base.OnPointerDown(eventData);
            EventSystem.current.SetSelectedGameObject(gameObject);

            

            bool success = false;
            if (discardButton.isDiscarding)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    handManager.lastSelectedCard = this;
                    success = discardButton.Discard(card);
                    if (success)
                    {
                        GlobalAudioManager.Instance.Play(
                        card.owner.ToString() + "Discard", Vector3.zero);
                    }
                }
                else
                {
                    success = true;
                }
            }
            else
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    GlobalAudioManager.Instance.Play("Tap", Vector3.zero);

                    success = CardPlayer.Instance.Play(card);
                }
                else if (eventData.button == PointerEventData.InputButton.Right)
                {
                    success = true;
                    inspectMenu.Enter(GetComponent<CardDisplay>(), card);
                }
                else
                {
                    success = true;
                }
            }
             

            if (!success)
            {
                rectTransform.DOShakeAnchorPos(0.3f, 20, 20)
                    .SetEase(Ease.OutQuint);
                EventSystem.current.SetSelectedGameObject(null);

                GlobalAudioManager.Instance.Play("Error", Vector3.zero);
            }
        }  
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        display.ShowOutline();
        if (IsInteractable())
        {
            rectTransform.DOSizeDelta(
                new Vector2(cardWidth * 1.3f, cardHeight * 1.3f), 0.3f)
                .SetEase(Ease.OutQuint);
        }  
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        display.HideOutline(cardMaterial);
        if (IsHighlighted())
        {
            rectTransform.DOSizeDelta(
            new Vector2(cardWidth * 1.2f, cardHeight * 1.2f), 0.3f)
            .SetEase(Ease.OutQuint);
        }
        else
        {
            rectTransform.DOSizeDelta(
            new Vector2(cardWidth, cardHeight), 0.3f)
            .SetEase(Ease.OutQuint);
            ZoomOut();
        }
    }

    public void SetInteractable(bool flag)
    {
        interactable = flag;
        if (flag)
        {
            if (IsHighlighted())
            {
                ZoomIn();
            }
            FadeIn();
        }
        else
        {
            FadeOut();
        }   
    }

    private void FadeIn()
    {
        DOTween.To(() => cardMaterial.GetFloat("_Saturation"),
                    (x) => cardMaterial.SetFloat("_Saturation", x),
                    1f, fadeTime).SetEase(Ease.OutQuint);
        group.DOFade(1f, fadeTime).SetEase(Ease.OutQuint);
    }

    private void FadeOut()
    {
        if (this != handManager.lastSelectedCard)
        {
            DOTween.To(() => cardMaterial.GetFloat("_Saturation"),
                    (x) => cardMaterial.SetFloat("_Saturation", x),
                    0.2f, fadeTime).SetEase(Ease.OutQuint);
            group.DOFade(0.8f, fadeTime).SetEase(Ease.OutQuint);
        }
    }

    private void ZoomIn()
    {
        rectTransform.DOSizeDelta(
                    new Vector2(cardWidth * 1.2f, cardHeight * 1.2f), 0.3f)
                    .SetEase(Ease.OutQuint);

        hand = handManager.GetComponentsInChildren<CardClick>().ToList();
        var index = hand.IndexOf(this);

        for (int i = 0; i < hand.Count; i++)
        {
            var indexDiff = i - index;
            if (indexDiff != 0)
            {
                var distance = Utils.BellCurve(indexDiff, 0, 1) * zoomFactor;
                if (indexDiff < 0)
                {
                    distance *= -1;
                }

                var position = new Vector2(
                    (hand.Count - 1 - i) * xSpacing + xOffset + distance,
                    cardYPos);

                hand[i].GetComponent<RectTransform>()
                    .DOAnchorPos(position,0.3f).SetEase(Ease.OutQuint);
            } 
        }
    }

    private void ZoomOut()
    {
        hand = handManager.GetComponentsInChildren<CardClick>().ToList();
        var index = hand.IndexOf(this);

        for (int i = 0; i < hand.Count; i++)
        {
            var indexDiff = i - index;
            if (indexDiff != 0)
            {

                var position = new Vector2(
                    (hand.Count - 1 - i) * xSpacing + xOffset,
                    cardYPos);

                hand[i].GetComponent<RectTransform>()
                        .DOAnchorPos(position, 0.3f).SetEase(Ease.OutQuint);
            }
        }
    }
}