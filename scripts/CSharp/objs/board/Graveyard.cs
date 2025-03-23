using Godot;

using System.Collections.Generic;

public partial class Graveyard : Node
{
    // Lista de cartas no cemitério
    private List<Card> cards = new List<Card>();


    public void AddToGrave(Card card)
    {
        cards.Add(card);
        GD.Print("Carta enviada ao cemitério: ", card.CardData["name"]);
    }


    public Card RetrieveFromGrave(int index)
    {
        if (index >= 0 && index < cards.Count)
        {
            Card retrievedCard = cards[index];
            cards.RemoveAt(index);
            return retrievedCard;
        }
        return null;
    }

    public void ClearGrave()
    {
        cards.Clear();
        GD.Print("Cemitério limpo.");
    }
}