using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CardDisplay : MonoBehaviour
{
    public Card card { get; private set; }

    private TextMeshProUGUI cardName;
    private CardClick cardClick;

    // Start is called before the first frame update

    public void SetCard(Card c)
    {
        card = c;
        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        cardName = GetComponentsInChildren<TextMeshProUGUI>()
                    .Where(c => c.gameObject.name == "CardName")
                    .FirstOrDefault();
        cardName.text = card.cardName;
    }
}
