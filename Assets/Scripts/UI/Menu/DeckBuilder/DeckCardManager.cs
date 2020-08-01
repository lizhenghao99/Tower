using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TowerUtils;
using UnityEngine;

public class DeckCardManager : MonoBehaviour
{
    [Header("Deck")]
    [SerializeField] public int cardMaxCount = 10;
    [Header("Owner")]
    [SerializeField] Card.Owner owner;
    [SerializeField] DeckCardClick cardPrefab;
    [Header("Card Dimensions")]
    [SerializeField] float cardXOffset = -100;
    [SerializeField] float cardXSpacing = -120;
    [SerializeField] float cardYPos = 100;
    [SerializeField] float cardWidth = 150;
    [SerializeField] float cardHeight = 250;
    [SerializeField] float zoomFactor = 500f;

    public List<DeckCardClick> deckCards;


    private void Start()
    {
        Card[] ownCard = GetComponentInParent<DeckBuilder>().ownCards;
        deckCards = new List<DeckCardClick>();
        var data = SaveSystem.LoadDeck();

        if (data != null && data.playerDecks.ContainsKey((int)owner))
        {
            foreach (string name in data.playerDecks[(int)owner])
            {
                AddCard(ownCard.Where(c => c.cardName == name).FirstOrDefault());
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

        var cardObject = Instantiate(cardPrefab, gameObject.transform, false);
        InitializeCardPos(cardObject);
        cardObject.SetCard(cardToAdd);
        cardObject.GetComponent<CardDisplay>().SetCard(cardToAdd);
        cardObject.GetComponent<RectTransform>().sizeDelta =
            new Vector2(cardWidth, cardHeight);


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

        Destroy(cardToRemove.gameObject, removeTime+0.1f);
        StartCoroutine(Utils.Timeout(() =>
        {
            foreach (DeckCardClick c in deckCards)
            {
                c.SetInteractable(true);
            }
        }, removeTime+0.1f));
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
}
