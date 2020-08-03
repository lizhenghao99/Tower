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

    private Image art;
    private TextMeshProUGUI cardName;
    private TextMeshProUGUI primaryResource;
    private TextMeshProUGUI secondaryResrouce;

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

        switch (card.owner)
        {
            case Card.Owner.Luban:
                primaryResource = GetComponentsInChildren<TextMeshProUGUI>()
                    .Where(c => c.gameObject.name == "PrimaryResource")
                    .FirstOrDefault();
                primaryResource.text =
                    card.primaryChange < 0 ? 
                    (-card.primaryChange/20).ToString() : "0";

                secondaryResrouce = GetComponentsInChildren<TextMeshProUGUI>()
                            .Where(c => c.gameObject.name == "SecondaryResource")
                            .FirstOrDefault();
                secondaryResrouce.text =
                    card.secondaryChange < 0 ?
                    (-card.secondaryChange).ToString() : "0";
                break;
            case Card.Owner.secondChar:
                break;
            case Card.Owner.thirdChar:
                break;
            default:
                break;
        }
    }
}
