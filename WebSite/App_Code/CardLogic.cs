﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CardLogic
/// </summary>
public class CardLogic
{
    public int index { get; set; }
    public List<Card> cards { get; set; }
    public List<Card> smallCardPool { get; set; }

     public List<Deck> decks { get; set; }
     public List<Card> cardsInDeck { get; set; }

    private DataAccessLayer layer = new DataAccessLayer();

    public CardLogic()
    {
        index = 0;
        cards = getCards();
        smallCardPool = new List<Card>();
    }

    
    public void setDecks(int userId)
    {
        decks = layer.readAllUserDecksFromDB(userId);
    }

    public void setCardsInActiveDeck(int deckId)
    {
        cardsInDeck.Clear();

        foreach (var item in decks.Find(x => x.deckId == deckId).cards)
        {
            for (int i = 0; i<item.count; i++)
            {
                cardsInDeck.Add(cards.Find(x => x.cardId == item.cardId));
            }
        }
     }



    private List<Card> getCards()
    {
        List<Card> temp = new List<Card>();

        try
        {
            temp = layer.readCardsFromDB();
            temp.Sort(new cardComparer());
            return temp;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void divideAndConquer(string name, int shortCut)
    {
        List<Card> temp = new List<Card>();
        bool foundFirst = false;
        index = 0;

        for (int i = shortCut; i < cards.Count; i++)
        {
            if (i == cards.Count - 1)
            {
                smallCardPool = temp;
                break;
            }

            if(cards[i].playerClass.Equals(name))
            {
                foundFirst = true;
                temp.Add(cards[i]);
            }
            else if (foundFirst)
            {              
                smallCardPool = temp;
                break;
            }
        }        
    }
}

public class cardComparer : IComparer<Card>
{
    public int Compare(Card x, Card y)
    {
        if (string.Compare(x.playerClass, y.playerClass) == 0)
        {
            if (x.cost > y.cost)
                return 1;
            else if (x.cost < y.cost)
                return -1;
            else
                return 0;
        }
        else if (string.Compare(x.playerClass, y.playerClass) == 1)
            return 1;
        else
            return -1;
    }
}