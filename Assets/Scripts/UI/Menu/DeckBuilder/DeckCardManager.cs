using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectTower;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectTower
{
    public class DeckCardManager : MonoBehaviour
    {
        [Header("Deck")]
        [SerializeField] public int cardMaxCount = 10;
        [Header("Owner")]
        [SerializeField] public Card.Owner owner;
        [SerializeField] public GameObject cardPrefab;
        [Header("Card Dimensions")]
        [SerializeField] float cardXOffset = 700;
        [SerializeField] float cardXSpacing = 250;
        [SerializeField] float cardYPos = 100;
        [SerializeField] float cardWidth = 299;
        [SerializeField] float cardHeight = 390;
        [SerializeField] float zoomFactor = 350f;

        public List<DeckCardClick> deckCards;

        public void SetOwner(Card.Owner o, GameObject c)
        {
            owner = o;
            cardPrefab = c;
            Card[] ownCard = GetComponentInParent<DeckBuilder>().ownCards;
            deckCards = new List<DeckCardClick>();
            var data = SaveSystem.Load();

            if (data.playerDecks.ContainsKey((int)owner)
                && data.playerDecks[(int)owner].Length > 0)
            {
                foreach (string name in data.playerDecks[(int)owner])
                {
                    AddCard(ownCard.Where(cc => cc.cardName == name
                                            && cc.upgraded == data.cardsUpgrade[name]
                                            ).FirstOrDefault());
                }
            }

            ShowInitialCards();
        }

        private void ShowInitialCards()
        {
            foreach (DeckCardClick c in deckCards)
            {
                c.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(cardXOffset + deckCards.IndexOf(c) * cardXSpacing,
                                cardYPos);
            }
        }

        public bool AddCard(Card cardToAdd)
        {
            float addTime = 0.4f;

            if (deckCards.Count == cardMaxCount) return false;

            var cardObject = Instantiate(cardPrefab, gameObject.transform, false)
                .AddComponent<DeckCardClick>();

            InitializeCardPos(cardObject);
            cardObject.SetCard(cardToAdd);
            cardObject.GetComponent<CardDisplay>().SetCard(cardToAdd);
            cardObject.GetComponent<RectTransform>().sizeDelta =
                new Vector2(cardWidth, cardHeight);
            cardObject.GetComponent<RectTransform>().anchorMin =
                new Vector2(0, 0f);
            cardObject.GetComponent<RectTransform>().anchorMax =
                new Vector2(0, 0f);


            if (deckCards.Count == 0)
            {
                deckCards.Add(cardObject);
                cardObject.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(cardXOffset, -cardYPos);
            }
            else
            {
                deckCards.Add(cardObject);
                deckCards = deckCards
                    .OrderByDescending(c => Math.Min(c.card.secondaryChange, 0))
                    .ThenByDescending(c => Math.Min(c.card.primaryChange, 0))
                    .ThenBy(c => c.card.cardName)
                    .ToList();

                var index = deckCards.IndexOf(cardObject);
                cardObject.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(cardXOffset + index * cardXSpacing, -cardYPos);

                foreach (DeckCardClick c in deckCards.Skip(index + 1))
                {
                    c.GetComponent<RectTransform>().DOAnchorPosX(
                        cardXOffset + (deckCards.IndexOf(c)) * cardXSpacing,
                        addTime)
                   .SetEase(Ease.OutQuint);
                }
            }

            cardObject.GetComponent<RectTransform>().DOAnchorPosY(cardYPos, addTime)
                .SetEase(Ease.OutQuint);

            for (int i = 0; i < deckCards.Count; i++)
            {
                deckCards[i].transform.SetSiblingIndex(i);
            }

            return true;
        }

        public void RemoveCard(DeckCardClick cardToRemove)
        {
            float removeTime = 0.3f;
            foreach (DeckCardClick c in deckCards)
            {
                c.SetInteractable(false);
            }

            cardToRemove.transform.DOLocalMoveY(300, removeTime)
                                        .SetRelative(true)
                                        .SetEase(Ease.OutQuint);
            cardToRemove.GetComponent<CanvasGroup>().DOFade(0f, removeTime)
                                        .SetEase(Ease.OutQuint);

            var index = deckCards.IndexOf(cardToRemove);
            foreach (DeckCardClick c in deckCards.Skip(index + 1))
            {
                c.GetComponent<RectTransform>().DOAnchorPosX(
                    cardXOffset + (deckCards.IndexOf(c) - 1) * cardXSpacing, removeTime)
                    .SetEase(Ease.OutQuint);
            }

            deckCards.Remove(cardToRemove);

            Destroy(cardToRemove.gameObject, removeTime + 0.1f);
            StartCoroutine(Utils.Timeout(() =>
            {
                foreach (DeckCardClick c in deckCards)
                {
                    c.SetInteractable(true);
                }
            }, removeTime + 0.1f));
        }

        public void ClearDeck()
        {
            float clearTime = 0.3f;
            foreach (DeckCardClick c in deckCards)
            {
                c.SetInteractable(false);
                c.transform.DOLocalMoveY(300, clearTime)
                                        .SetRelative(true)
                                        .SetEase(Ease.OutQuint);
                c.GetComponent<CanvasGroup>().DOFade(0f, clearTime)
                                            .SetEase(Ease.OutQuint);
                Destroy(c.gameObject, clearTime + 0.1f);
            }
            deckCards.Clear();
        }

        private void InitializeCardPos(DeckCardClick c)
        {
            c.xOffset = cardXOffset;
            c.xSpacing = cardXSpacing;
            c.cardYPos = cardYPos;
            c.cardWidth = cardWidth;
            c.cardHeight = cardHeight;
            c.zoomFactor = zoomFactor;
        }

        public void CleanUp()
        {
            foreach (DeckCardClick c in deckCards)
            {
                Destroy(c.gameObject, 0.5f);
            }
        }
    }
}