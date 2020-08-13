using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LubanCardDisplay : CardDisplay
{
    protected override void DisplayResource()
    {
        primaryResource = GetComponentsInChildren<TextMeshProUGUI>()
                    .Where(c => c.gameObject.name == "PrimaryResource")
                    .FirstOrDefault();
        primaryResource.text =
            card.primaryChange < 0 ?
            (-card.primaryChange / 20).ToString() : "0";

        secondaryResrouce = GetComponentsInChildren<TextMeshProUGUI>()
                    .Where(c => c.gameObject.name == "SecondaryResource")
                    .FirstOrDefault();
        secondaryResrouce.text =
            card.secondaryChange < 0 ?
            (-card.secondaryChange).ToString() : "0";
    }
}
