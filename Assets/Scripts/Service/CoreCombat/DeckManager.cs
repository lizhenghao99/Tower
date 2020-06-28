using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerUtils;

public class DeckManager : Singleton<DeckManager>
{
    public Card.Owner firstOwner;
    public Card.Owner secondOwner;
    public Card.Owner thirdOwner;

    public List<Card> deck1;
    public List<Card> deck2;
    public List<Card> deck3;

    private Stack<Card> drawPile1;
    private Stack<Card> drawPile2;
    private Stack<Card> drawPile3;

    private List<Card> discardPile1;
    private List<Card> discardPile2;
    private List<Card> discardPile3;

    private Dictionary<Card.Owner, List<Card>> deck;
    private Dictionary<Card.Owner, Stack<Card>> drawPile;
    private Dictionary<Card.Owner, List<Card>> discardPile;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCombat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCombat()
    {
        deck = new Dictionary<Card.Owner, List<Card>>();
        drawPile = new Dictionary<Card.Owner, Stack<Card>>();
        discardPile = new Dictionary<Card.Owner, List<Card>>();

        drawPile1 = new Stack<Card>(deck1.Shuffle());
        drawPile2 = new Stack<Card>(deck2.Shuffle());
        drawPile3 = new Stack<Card>(deck3.Shuffle());

        discardPile1 = new List<Card>();
        discardPile2 = new List<Card>();
        discardPile3 = new List<Card>();


        deck.Add(firstOwner, deck1);
        deck.Add(secondOwner, deck2);
        deck.Add(thirdOwner, deck3);

        drawPile.Add(firstOwner, drawPile1);
        drawPile.Add(secondOwner, drawPile2);
        drawPile.Add(thirdOwner, drawPile3);

        discardPile.Add(firstOwner, discardPile1);
        discardPile.Add(secondOwner, discardPile2);
        discardPile.Add(thirdOwner, discardPile3);
    }

    public Card DrawCard(Card.Owner owner)
    { 
        if (deck[owner] == null) return null;

        if (drawPile[owner].Count <= 0)
        {
            drawPile[owner] = new Stack<Card>(discardPile[owner].Shuffle());
            discardPile[owner].Clear();
        }

        var newCard = drawPile[owner].Pop();

        return newCard;
    }

    public void DiscardCard(Card c)
    {
        discardPile[c.owner].Add(c);
    }
}
