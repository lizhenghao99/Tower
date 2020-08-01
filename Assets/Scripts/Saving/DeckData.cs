using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeckData
{
    public Dictionary<int, string[]> playerDecks;

    public DeckData()
    {
        playerDecks = new Dictionary<int, string[]>();
    }
}
