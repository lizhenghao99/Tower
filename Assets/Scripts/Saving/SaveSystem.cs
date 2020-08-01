using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public static class SaveSystem
{
    public static void SaveDeck(Card.Owner owner, Card[] cards)
    {
        var currDeck = LoadDeck();
        if (currDeck == null)
        {
            currDeck = new DeckData();
        }
        currDeck.playerDecks[(int)owner] = 
            cards.Select(c => c.cardName).ToArray();

        string path = Application.persistentDataPath + "/deck.save";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, currDeck);
        stream.Close();
    }

    public static DeckData LoadDeck()
    {
        string path = Application.persistentDataPath + "/deck.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;
            DeckData data = formatter.Deserialize(stream) as DeckData;

            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
}
