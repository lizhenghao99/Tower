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

public class CollectionCardClick : Selectable
{
    public Card card { get; private set; }

    public float xOffset;
    public float xSpacing;
    public float cardYPos;
    public float cardWidth;
    public float cardHeight;
    public float zoomFactor;

    public CanvasGroup group;

    private RectTransform rectTransform;

    private float fadeTime = 0.3f;

    private InspectMenu inspectMenu;
    private DeckCardManager deckCardManager;

    protected override void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    protected override void Start()
    {
        base.Start();
        rectTransform = GetComponent<RectTransform>();
        inspectMenu = FindObjectOfType<InspectMenu>();
        deckCardManager = FindObjectOfType<DeckCardManager>();
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
            
            ZoomOut();
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);

        ZoomOut();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (IsInteractable())
        {
            base.OnPointerDown(eventData);

            bool success = false;
            

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                GlobalAudioManager.Instance.Play("Tap", Vector3.zero);
                success = deckCardManager.AddCard(card);
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

            if (!success)
            {
                rectTransform.DOShakeAnchorPos(0.3f, 20, 20)
                    .SetEase(Ease.OutQuint);
                EventSystem.current.SetSelectedGameObject(null);

                GlobalAudioManager.Instance.Play("Error", Vector3.zero);
            }
            else
            {
                ClickZoom();
            }
        }
    }

    private void ZoomIn()
    {
        rectTransform.DOSizeDelta(
                    new Vector2(cardWidth * 1.2f, cardHeight * 1.2f), 0.3f)
                    .SetEase(Ease.OutQuint);
    }

    private void ZoomOut()
    {
        rectTransform.DOSizeDelta(
                new Vector2(cardWidth, cardHeight), 0.3f)
                .SetEase(Ease.OutQuint);
    }

    private void ClickZoom()
    {
        rectTransform.DOSizeDelta(
                    new Vector2(cardWidth * 1.25f, cardHeight * 1.25f), 0.1f)
                    .SetEase(Ease.OutQuint)
                    .OnComplete(() => rectTransform.DOSizeDelta(
                        new Vector2(cardWidth * 1.2f, cardHeight * 1.2f), 0.1f)
                        .SetEase(Ease.OutQuint));
    }
}