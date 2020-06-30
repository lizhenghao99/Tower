using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TowerUtils;
using TMPro;

public class HandManager : MonoBehaviour
{
    [Header("Owner")]
    [SerializeField] Card.Owner owner;
    [SerializeField] CardClick cardPrefab;
    [Header("Card Dimensions")]
    [SerializeField] float cardXOffset = -100;
    [SerializeField] float cardXSpacing = -120;
    [SerializeField] float cardYPos = 100;
    [SerializeField] float cardWidth = 150;
    [SerializeField] float cardHeight = 250;
    
    public CardClick lastSelectedCard { get; private set; }
    public PlayerController player;
    private List<CardClick> hand;

    private float fadeTime = 0.1f;
    private float zoomFactor = 500f;

    private void Awake()
    {
        player = FindObjectsOfType<PlayerController>()
                .Where(p => p.gameObject.name == owner.ToString())
                .FirstOrDefault();
        player.startCasting += OnStartCasting;
        player.finishCasting += OnFinishCasting;
    }

    private void Start()
    {

    }

    public void StartCombat()
    {
        StartCoroutine(DrawInitialCards());
    }

    private IEnumerator DrawInitialCards()
    {
        for (int i = 0; i < 5; i++)
        {
            var card = Instantiate(cardPrefab, gameObject.transform);
            InitializeCardPos(card);
            card.interactable = false;

            var newCard = DeckManager.Instance.DrawCard(owner);
            card.SetCard(newCard);
            card.GetComponent<CardDisplay>().SetCard(newCard);

            var rectTransform = card.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(cardXOffset, 0);

            rectTransform.DOAnchorPosY(cardYPos, 0.3f)
                .SetEase(Ease.OutQuint)
                .OnComplete(() =>
                    rectTransform.DOAnchorPosX(
                        cardXSpacing * (4 - i) + cardXOffset, 1f)
                    .SetEase(Ease.OutQuint)
                );
            yield return new WaitForSecondsRealtime(0.5f);
        }
        hand = GetComponentsInChildren<CardClick>().ToList();
        foreach (CardClick c in hand)
        {
            c.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var selected = EventSystem.current.currentSelectedGameObject;
        if (selected)
        {
            var selectedCard = selected.GetComponent<CardClick>();
            if (selectedCard)
            {
                lastSelectedCard = selectedCard;
            }
        }
    }

    private void OnStartCasting(object sender, EventArgs e)
    {
        hand = GetComponentsInChildren<CardClick>().ToList();
        foreach (CardClick c in hand)
        {
            c.SetInteractable(false);
        }
        Invoke("StartCardAnimation", 0.3f);
    }

    private void OnFinishCasting(object sender, EventArgs e)
    {
        hand = GetComponentsInChildren<CardClick>().ToList();
        foreach (CardClick c in hand)
        {
            c.SetInteractable(true);
        }
    }

    private void StartCardAnimation()
    {
        lastSelectedCard.transform.DOLocalMoveY(300, 0.4f)
                                    .SetRelative(true)
                                    .SetEase(Ease.OutQuint)
                                    .OnComplete(CompleteFade);

        var index = hand.IndexOf(lastSelectedCard);
        foreach(CardClick c in hand.Skip(index+1))
        {
            c.transform.DOLocalMoveX(cardXSpacing, 1f)
                        .SetRelative(true)
                        .SetEase(Ease.OutQuint);
        }
        Destroy(lastSelectedCard.gameObject, 0.6f);
        //lastSelectedCard.transform.SetParent(gameObject.transform.parent, true);
        StartCoroutine(Utils.Timeout(
            () => {
                var card = Instantiate(cardPrefab, gameObject.transform);
                card.interactable = false;
                InitializeCardPos(card);

                var newCard = DeckManager.Instance.DrawCard(owner);
                card.SetCard(newCard);
                card.GetComponent<CardDisplay>().SetCard(newCard);

                var rectTransform = card.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(-cardYPos, 0);

                foreach (TextMeshProUGUI text in card.GetComponentsInChildren<TextMeshProUGUI>())
                {
                    text.DOFade(0.7f, fadeTime).SetEase(Ease.OutQuint);
                }
                foreach (Image i in card.GetComponentsInChildren<Image>())
                {
                    i.DOFade(0.7f, fadeTime).SetEase(Ease.OutQuint);
                }
                rectTransform.DOAnchorPosY(cardYPos, 1).SetEase(Ease.OutQuint);
            }, 0.3f));
    }

    private void CompleteFade()
    {
        foreach (TextMeshProUGUI text in lastSelectedCard.GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.DOFade(0f, 0.2f).SetEase(Ease.OutQuint);
        }
        foreach (Image i in lastSelectedCard.GetComponentsInChildren<Image>())
        {
            i.DOFade(0f, 0.2f).SetEase(Ease.OutQuint);
        }
    }

    private void InitializeCardPos(CardClick c)
    {
        c.xOffset = cardXOffset;
        c.xSpacing = cardXSpacing;
        c.cardYPos = cardYPos;
        c.cardWidth = cardWidth;
        c.cardHeight = cardHeight;
    }
}
