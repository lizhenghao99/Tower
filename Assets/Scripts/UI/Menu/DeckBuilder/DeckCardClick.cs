using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Linq;
using ProjectTower;

namespace ProjectTower
{
    public class DeckCardClick : Selectable
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
        private DeckCardManager deckCardManager;

        private float fadeTime = 0.3f;

        private List<DeckCardClick> deckCards;

        private InspectMenu inspectMenu;

        protected override void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        protected override void Start()
        {
            base.Start();
            rectTransform = GetComponent<RectTransform>();
            deckCardManager = GetComponentInParent<DeckCardManager>();
            inspectMenu = FindObjectOfType<InspectMenu>();
            deckCards = deckCardManager.deckCards;
        }

        public void SetCard(Card c)
        {
            card = c;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            ZoomIn();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            rectTransform.DOSizeDelta(
                    new Vector2(cardWidth, cardHeight), 0.3f)
                    .SetEase(Ease.OutQuint);
            ZoomOut();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (IsInteractable())
            {
                base.OnPointerDown(eventData);
                EventSystem.current.SetSelectedGameObject(null);
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    GlobalAudioManager.Instance.Play("Place", Vector3.zero);
                    deckCardManager.RemoveCard(this);
                }
                else
                {
                    inspectMenu.Enter(GetComponent<CardDisplay>(), card);
                }
            }
        }

        public void SetInteractable(bool flag)
        {
            interactable = flag;
            if (flag && IsHighlighted())
            {
                ZoomIn();
            }
        }

        private void ZoomIn()
        {
            deckCards = deckCardManager.deckCards;
            rectTransform.DOSizeDelta(
                        new Vector2(cardWidth * 1.2f, cardHeight * 1.2f), 0.3f)
                        .SetEase(Ease.OutQuint);

            var index = deckCards.IndexOf(this);

            for (int i = 0; i < deckCards.Count; i++)
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
                        i * xSpacing + xOffset + distance,
                        cardYPos);

                    deckCards[i].GetComponent<RectTransform>()
                        .DOAnchorPos(position, 0.3f).SetEase(Ease.OutQuint);
                }
            }
        }

        private void ZoomOut()
        {
            deckCards = deckCardManager.deckCards;
            var index = deckCards.IndexOf(this);

            for (int i = 0; i < deckCards.Count; i++)
            {
                var indexDiff = i - index;
                if (indexDiff != 0)
                {

                    var position = new Vector2(
                        i * xSpacing + xOffset,
                        cardYPos);

                    deckCards[i].GetComponent<RectTransform>()
                            .DOAnchorPos(position, 0.3f).SetEase(Ease.OutQuint);
                }
            }
        }
    }
}