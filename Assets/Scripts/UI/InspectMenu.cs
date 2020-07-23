using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;

public class InspectMenu : MonoBehaviour
{
    [SerializeField] GameObject inspectMenu;
    [SerializeField] Vector2 cardPos = new Vector2(-1000, 250);

    public static bool isInspecting = false;
    public static bool isPaused = false;

    private CanvasGroup group;
    private CardDisplay card;


    // Start is called before the first frame update
    void Start()
    {
        group = inspectMenu.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInspecting)
        {
            if (isPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        } 
    }

    public void Enter(CardDisplay c, Card cardInfo)
    {
        isInspecting = true;
        isPaused = true;
        Time.timeScale = 0f;
        inspectMenu.SetActive(true);
        group.DOFade(1f, 0.2f).SetEase(Ease.OutQuint).SetUpdate(true);

        card = Instantiate(c, inspectMenu.transform);
        card.SetCard(cardInfo);
        card.GetComponent<CardClick>().enabled = false;

        foreach (TextMeshProUGUI text in card.GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.fontSizeMax = 200;
        }

        foreach (Image image in card.GetComponentsInChildren<Image>())
        {
            image.material.SetFloat("_HsvSaturation", 1f);
        }

        var rectTransform = card.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

        rectTransform.anchoredPosition = cardPos;
        rectTransform.sizeDelta *= 3;

        inspectMenu.GetComponentInChildren<TextMeshProUGUI>().text =
            card.card.description;

        GlobalAudioManager.Instance.Play("Inspect", Vector3.zero);
    }

    public void Exit()
    {
        if (card != null)
        {
            Destroy(card.gameObject);
        }

        isInspecting = false; 
        isPaused = false;
        Time.timeScale = 1f;

        group.DOFade(0f, 0.2f).SetEase(Ease.OutQuint).SetUpdate(true)
            .OnComplete(() => inspectMenu.SetActive(false));
    }
}
