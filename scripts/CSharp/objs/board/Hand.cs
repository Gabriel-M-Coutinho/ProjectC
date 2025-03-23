using Godot;
using System;
using System.Collections.Generic;

public partial class Hand : Node
{
    // Lista de cartas na mão
    private List<Card> cards = new List<Card>();

    // Número máximo de cartas na mão
    private const int MAX_CARDS = 6;

    // Método para adicionar uma carta à mão
    public void AddCard(Card card)
    {

        if (cards.Count < MAX_CARDS)
        {
            cards.Add(card);
            GD.Print("Carta adicionada à mão: ", card.CardData["name"]);
        }
        else
        {
            GD.Print("Mão cheia! Descarte antes de comprar.");
        }
    }

    // Método para remover uma carta da mão
    public Card RemoveCard(Card card)
    {
        if (card == null)
        {
            GD.Print("Tentativa de remover uma carta nula!");
            return null;
        }

        // Verifica se a carta está na lista
        int index = cards.FindIndex(c => c.CardId == card.CardId);

        if (index >= 0)
        {
            Card removedCard = cards[index];
            cards.RemoveAt(index);
            GD.Print("Carta removida da mão: ", removedCard.CardData["name"]);
            return removedCard;
        }
        else
        {
            GD.Print("A carta não foi encontrada na mão!");
            return null;
        }
    }

    // Método para obter o número de cartas na mão
    public int Size()
    {
        return cards.Count;
    }

    // Método para ordenar as cartas na mão
    public void SortHand()
    {
        cards.Sort((card1, card2) =>
        {
            // Aqui você pode definir a lógica de ordenação.
            // Por exemplo, ordenar por nome, ataque, vida, etc.
            string name1 = (string)card1.CardData["name"];
            string name2 = (string)card2.CardData["name"];
            return name1.CompareTo(name2);
        });
    }
}