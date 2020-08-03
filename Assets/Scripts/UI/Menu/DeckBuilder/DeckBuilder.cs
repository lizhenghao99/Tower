using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class DeckBuilder : MonoBehaviour
{
    [Header("Deck")]
    [SerializeField] Card.Owner owner;
    [SerializeField] GameObject collectionCardPrefab;
    [Header("UI")]
    [SerializeField] int cardsPerPage = 8;
    [SerializeField] TextMeshProUGUI cardCountText;
    [SerializeField] Button confirmButton;
    [SerializeField] TextMeshProUGUI confirmButtonWarning;
    [SerializeField] Button nextPage;
    [SerializeField] Button lastPage;
    [SerializeField] Button resetDeck;

    [HideInInspector] public Card[] cards;
    [HideInInspector] public int collectionCardCount;

    public Card[] ownCards;
    private GridLayoutGroup grid;
    private DeckCardManager deckCardManager;
    private int currPage = 0;
    private Vector2 gridPosition;

    private float shiftDistance = -3000;

    private void Awake()
    {
        cards = Resources.LoadAll<Card>("Cards");
        grid = GetComponentInChildren<GridLayoutGroup>();
        gridPosition = grid.GetComponent<RectTransform>().anchoredPosition;
        deckCardManager = GetComponentInChildren<DeckCardManager>();

        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowPage(currPage);
    }

    // Update is called once per frame
    void Update()
    {
        cardCountText.text = deckCardManager.deckCards.Count.ToString();
        if (deckCardManager.deckCards.Count == deckCardManager.cardMaxCount)
        {
            confirmButton.interactable = true;
            confirmButtonWarning.enabled = false;
        }
        else
        {
            confirmButton.interactable = false;
            confirmButtonWarning.enabled = true;
        }

        if (currPage == 0)
        {
            lastPage.gameObject.SetActive(false);
        }
        else
        {
            lastPage.gameObject.SetActive(true);
        }

        if (currPage == collectionCardCount / cardsPerPage)
        {
            nextPage.gameObject.SetActive(false);
        }
        else
        {
            nextPage.gameObject.SetActive(true);
        }
    }

    private void ShowPage(int p)
    {
        float animationTime = 0.3f;
        var previousCards = grid.GetComponentsInChildren<CollectionCardClick>();
        foreach (CollectionCardClick c in previousCards)
        {
            c.GetComponent<LayoutElement>().ignoreLayout = true;
            c.GetComponent<CanvasGroup>().DOFade(0f, animationTime/2)
                .SetEase(Ease.OutQuint);
            Destroy(c.gameObject, animationTime + 0.1f);
        }
        for (int i = p*cardsPerPage; 
            i < Math.Min((p+1)*cardsPerPage, collectionCardCount); 
            i++)
        {
            var cardObject = Instantiate(collectionCardPrefab,
                grid.gameObject.transform)
                .AddComponent<CollectionCardClick>();
            cardObject.gameObject.AddComponent<LayoutElement>();
            cardObject.SetCard(ownCards[i]);
            cardObject.GetComponent<CardDisplay>().SetCard(ownCards[i]);
            cardObject.cardWidth = grid.cellSize.x;
            cardObject.cardHeight = grid.cellSize.y;
            cardObject.GetComponent<CanvasGroup>().DOFade(1f, animationTime / 2)
                .SetEase(Ease.OutQuint);
        }
        grid.GetComponent<RectTransform>().anchoredPosition +=
            new Vector2(-shiftDistance, 0);
        grid.GetComponent<RectTransform>()
            .DOAnchorPos(gridPosition, animationTime)
            .SetEase(Ease.OutQuint);
    }

    public void ShowNextPage()
    {
        shiftDistance = -Math.Abs(shiftDistance);
        currPage++;
        ShowPage(currPage);
    }

    public void ShowLastPage()
    {
        shiftDistance = Math.Abs(shiftDistance);
        currPage--;
        ShowPage(currPage);
    }

    public void SaveDeck()
    {
        SaveSystem.SaveDeck(
            owner, deckCardManager.deckCards.Select(c => c.card).ToArray());
    }

    public void ResetDeck()
    {
        deckCardManager.ClearDeck();
    }

    private void Refresh()
    {
        var data = SaveSystem.Load();
        ownCards = cards.Where(c => c.owner == owner
                                && c.upgraded == data.cardsUpgrade[c.cardName])
            .OrderByDescending(c => Math.Min(c.secondaryChange, 0))
            .ThenByDescending(c => Math.Min(c.primaryChange, 0))
            .ThenBy(c => c.cardName)
            .ToArray();
        collectionCardCount = ownCards.Length;
    }
}
