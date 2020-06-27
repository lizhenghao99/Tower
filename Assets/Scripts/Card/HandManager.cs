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
    [SerializeField] Card.Owner owner;
    [SerializeField] CardClick cardPrefab;
    [SerializeField] float cardXPos;
    [SerializeField] float cardYPos;

    public CardClick lastSelectedCard { get; private set; }
    public PlayerController player;
    private HorizontalLayoutGroup group;
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
        group = GetComponentInParent<HorizontalLayoutGroup>();
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
        //group.enabled = false;
        Invoke("StartCardAnimation", 0.3f);
    }

    private void OnFinishCasting(object sender, EventArgs e)
    {
        hand = GetComponentsInChildren<CardClick>().ToList();
        foreach (CardClick c in hand)
        {
            c.SetInteractable(true);
        }
        //group.enabled = false;
        //group.enabled = true;
    }

    private void StartCardAnimation()
    {
        lastSelectedCard.transform.DOLocalMoveY(300, 0.5f)
                                    .SetRelative(true)
                                    .SetEase(Ease.OutQuint)
                                    .OnComplete(CompleteFade);

        var index = hand.IndexOf(lastSelectedCard);
        foreach(CardClick c in hand.Skip(index+1))
        {
            c.transform.DOLocalMoveX(-cardXPos, 1f)
                        .SetRelative(true)
                        .SetEase(Ease.OutQuint);
        }
        Destroy(lastSelectedCard.gameObject, 0.8f);
        //lastSelectedCard.transform.SetParent(gameObject.transform.parent, true);
        StartCoroutine(Utils.Timeout(
            () => {
                var card = Instantiate(cardPrefab, gameObject.transform);
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
            text.DOFade(0f, 0.3f).SetEase(Ease.OutQuint);
        }
        foreach (Image i in lastSelectedCard.GetComponentsInChildren<Image>())
        {
            i.DOFade(0f, 0.3f).SetEase(Ease.OutQuint);
        }
    }
}
