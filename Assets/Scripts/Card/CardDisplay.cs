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

    protected Image art;
    protected TextMeshProUGUI cardName;
    protected TextMeshProUGUI primaryResource;
    protected TextMeshProUGUI secondaryResrouce;

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
        if (card.upgraded)
        {
            cardName.text = card.cardName +
                "<voffset=.12em><size=30%> <size=70%>·<size=30%> <size=70%>精</voffset>";
        }
        else
        {
            cardName.text = card.cardName;
        }
        

        art = GetComponentsInChildren<Image>()
                    .Where(a => a.gameObject.name == "Art")
                    .FirstOrDefault();
        art.sprite = card.art;

        DisplayResource();
    }

    protected virtual void DisplayResource()
    {
        // do nothing
    }
}
