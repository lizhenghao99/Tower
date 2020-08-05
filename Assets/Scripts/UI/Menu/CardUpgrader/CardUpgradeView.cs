using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class CardUpgradeView : MonoBehaviour
{
    [SerializeField] Vector2 cardPos = new Vector2(-1000, 250);
    [SerializeField] Vector2 cardSize = new Vector2(230, 300) * 3.6f;

    [SerializeField] TextMeshProUGUI changeText;
    [SerializeField] TextMeshProUGUI costText;


    private Card cardToUpgrade;
    private CanvasGroup group;
    private CardDisplay oldCard;
    private CardDisplay newCard;
    private CardUpgrader upgrader;


    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        upgrader = FindObjectOfType<CardUpgrader>();
    }


    public void Enter(CardDisplay c, Card cardInfo)
    {
        cardToUpgrade = cardInfo;
        group.DOFade(1f, 0.2f).SetEase(Ease.OutQuint).SetUpdate(true);

        oldCard = Instantiate(c, gameObject.transform, false);
        newCard = Instantiate(c, gameObject.transform, false);
        oldCard.SetCard(cardInfo);
        newCard.SetCard(upgrader.cards.Where(ca => ca.cardName == cardInfo.cardName
            && ca.upgraded == true).FirstOrDefault());

        if (oldCard.GetComponent<Selectable>())
        {
            oldCard.GetComponent<Selectable>().enabled = false;
        }
        if (newCard.GetComponent<Selectable>())
        {
            newCard.GetComponent<Selectable>().enabled = false;
        }

        var oldCostText = oldCard.GetComponentsInChildren<TextMeshProUGUI>()
            .Where(t => t.transform.parent == oldCard.transform)
            .FirstOrDefault();
        if (oldCostText != null)
        {
            oldCostText.gameObject.SetActive(false);
        }

        var newCostText = newCard.GetComponentsInChildren<TextMeshProUGUI>()
            .Where(t => t.transform.parent == newCard.transform)
            .FirstOrDefault();
        if (newCostText != null)
        {
            newCostText.gameObject.SetActive(false);
        }


        foreach (TextMeshProUGUI text in oldCard.GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.fontSizeMax = 200;
        }
        foreach (TextMeshProUGUI text in newCard.GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.fontSizeMax = 200;
        }

        var rectTransform = oldCard.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = cardPos;
        rectTransform.sizeDelta = cardSize;

        rectTransform = newCard.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(-cardPos.x, cardPos.y);
        rectTransform.sizeDelta = cardSize;


        var info = upgrader
            .upgradeInfo.Where(i => i.cardName == cardInfo.cardName)
            .FirstOrDefault();
        changeText.text = info.upgradeDescription;
        costText.text = "消耗：" + info.upgradeCost + "金";

        GlobalAudioManager.Instance.Play("Inspect", Vector3.zero);
    }

    public void Exit()
    {
        if (oldCard != null)
        {
            Destroy(oldCard.gameObject);
        }

        group.DOFade(0f, 0.2f).SetEase(Ease.OutQuint).SetUpdate(true)
            .OnComplete(() => gameObject.SetActive(false));
        upgrader.resultView.GetComponent<CanvasGroup>()
            .DOFade(0f, 0.2f).SetEase(Ease.OutQuint).SetUpdate(true)
            .OnComplete(() => upgrader.resultView.SetActive(false));
    }

    public void Upgrade()
    {
        SaveSystem.SaveUpgrade(cardToUpgrade);
        upgrader.resultView.SetActive(true);
        upgrader.resultView.GetComponent<CanvasGroup>()
            .DOFade(1f, 0.2f).SetEase(Ease.OutQuint).SetUpdate(true);

        var resultCard = Instantiate(
            newCard, upgrader.resultView.transform, false);
        resultCard.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(0, cardPos.y);

        upgrader.resultView.GetComponentInChildren<TextMeshProUGUI>()
            .text = newCard.card.description;

        StartCoroutine(PlayBlackSmithSounds());
    }

    private IEnumerator PlayBlackSmithSounds()
    {
        GlobalAudioManager.Instance.Play("BlackSmith", Vector3.zero);
        yield return new WaitForSecondsRealtime(0.5f);
        GlobalAudioManager.Instance.Play("BlackSmith", Vector3.zero);
    }
}
