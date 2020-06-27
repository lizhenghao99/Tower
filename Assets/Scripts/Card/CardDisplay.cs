using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CardDisplay : MonoBehaviour
{
    public Card card;

    private TextMeshProUGUI cardName;
    private CardClick cardClick;

    // Start is called before the first frame update
    void Start()
    {
        cardName = GetComponentsInChildren<TextMeshProUGUI>()
                    .Where(c => c.gameObject.name == "CardName")
                    .FirstOrDefault();
        cardClick = GetComponent<CardClick>();
        cardName.text = card.cardName;
    }
}
