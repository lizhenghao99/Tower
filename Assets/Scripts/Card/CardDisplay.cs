using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    private TextMeshProUGUI cardName;

    private void Awake()
    {
        cardName = GetComponentsInChildren<TextMeshProUGUI>()
                    .Where(c => c.gameObject.name == "CardName")
                    .FirstOrDefault();
    }

    // Start is called before the first frame update
    void Start()
    {
        cardName.text = card.cardName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
