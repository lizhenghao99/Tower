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
        var data = Load();
        data.playerDecks[(int)owner] = 
            cards.Select(c => c.cardName).ToArray();

        string path = Application.persistentDataPath + "/player.save";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveUpgrade(Card card)
    {

    }

    public static PlayerData Load()
    {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;
            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();
            return data;
        }
        else
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            var data = new PlayerData();
            formatter.Serialize(stream, data);

            stream.Close();
            return data;
        }
    }
}
