using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class CardUpgrader : MonoBehaviour
{
    [Header("Deck")]
    [SerializeField] Card.Owner owner;
    [SerializeField] GameObject upgraderCardPrefab;
    [Header("UI")]
    [SerializeField] int cardsPerPage = 8;
    [SerializeField] Button nextPage;
    [SerializeField] Button lastPage;
    [SerializeField] CardUpgradeView upgradeView;
    [SerializeField] public GameObject resultView;
    [SerializeField] public TextMeshProUGUI upgradeCostText;

    [HideInInspector] public Card[] cards;
    [HideInInspector] public Card[] ownCards;
    [HideInInspector] public int collectionCardCount;
    [HideInInspector] public CardUpgradeInfo[] upgradeInfo;
    

    private GridLayoutGroup grid;
    private int currPage = 0;
    private Vector2 gridPosition;

    private float shiftDistance = -3000;

    private void Awake()
    {
        cards = Resources.LoadAll<Card>("Cards");
        upgradeInfo = Resources.LoadAll<CardUpgradeInfo>("CardUpgradeInfoList");
        grid = GetComponentInChildren<GridLayoutGroup>();
        gridPosition = grid.GetComponent<RectTransform>().anchoredPosition;

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
        var previousCards = grid.GetComponentsInChildren<UpgraderCardClick>();
        foreach (UpgraderCardClick c in previousCards)
        {
            c.GetComponent<LayoutElement>().ignoreLayout = true;
            c.GetComponent<CanvasGroup>().DOFade(0f, animationTime / 2)
                .SetEase(Ease.OutQuint);
            Destroy(c.gameObject, animationTime + 0.1f);
        }
        for (int i = p * cardsPerPage;
            i < Math.Min((p + 1) * cardsPerPage, collectionCardCount);
            i++)
        {
            var cardObject = Instantiate(upgraderCardPrefab,
                grid.gameObject.transform)
                .AddComponent<UpgraderCardClick>();
            cardObject.gameObject.AddComponent<LayoutElement>();
            cardObject.SetCard(ownCards[i]);
            cardObject.GetComponent<CardDisplay>().SetCard(ownCards[i]);
            cardObject.cardWidth = grid.cellSize.x;
            cardObject.cardHeight = grid.cellSize.y;
            cardObject.GetComponent<CanvasGroup>().DOFade(1f, animationTime / 2)
                .SetEase(Ease.OutQuint);

            var costText = Instantiate(upgradeCostText,
                cardObject.transform);
            costText.gameObject.SetActive(true);
            costText.text = upgradeInfo
                .Where(info => info.cardName == cardObject.card.cardName)
                .FirstOrDefault()?.upgradeCost.ToString();
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

    private void Refresh()
    {
        var data = SaveSystem.Load();
        ownCards = cards.Where(c => c.owner == owner 
                && !c.upgraded
                && !data.cardsUpgrade[c.cardName])
            .OrderByDescending(c => Math.Min(c.secondaryChange, 0))
            .ThenByDescending(c => Math.Min(c.primaryChange, 0))
            .ThenBy(c => c.cardName)
            .ToArray();
        collectionCardCount = ownCards.Length;
    }

    public void EnterUpgrade(CardDisplay c, Card cardInfo)
    {
        upgradeView.gameObject.SetActive(true);
        upgradeView.Enter(c, cardInfo);
    }

    public void ExitUpgrade()
    {
        upgradeView.Exit();
        Refresh();
        ShowPage(currPage);
    }
}
