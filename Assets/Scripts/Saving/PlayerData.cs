using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Dictionary<int, string[]> playerDecks;
    public Dictionary<string, bool> cardsUpgrade;

    public int money;

    public PlayerData()
    {
        var cards = Resources.LoadAll<Card>("Cards");
        playerDecks = new Dictionary<int, string[]>();
        cardsUpgrade = new Dictionary<string, bool>();
        foreach (Card c in cards)
        {
            cardsUpgrade[c.cardName] = false;
        }

        money = 0;
    }
}
