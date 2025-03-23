using Godot;
using System;
using System.Collections.Generic;

public partial class Deck : Node
{
    // Cena da carta (precarregada)
    private PackedScene CardScene = ResourceLoader.Load<PackedScene>("res://scenes/card.tscn");

    // Lista de cartas no deck
    private List<Card> cards = new List<Card>();

    // Contador para gerar IDs únicos
    private int nextCardId = 0;

    // Referência ao AnimationPlayer
    private AnimationPlayer animationPlayer;

    // Referência ao Board para adicionar a carta na mão
    private Board board;

    // CICLO DE VIDA
    public override void _Ready()
    {
        // Tenta acessar o Board usando o caminho absoluto
        board = GetParent() as Board;

    }

    // Método para carregar o deck com dados
    public void LoadDeck(List<Dictionary<string, object>> deckData, int playerId)
    {
        foreach (var cardData in deckData)
        {
            // Instancia uma nova carta a partir da cena
            Card newCard = CardScene.Instantiate<Card>();
            if (newCard != null)
            {
                newCard.Setup(cardData, nextCardId, playerId);
                cards.Add(newCard);
                nextCardId++;
            }
            else
            {
                GD.PrintErr("Erro ao instanciar a carta.");
            }
        }

        GD.Print("Cartas carregadas no deck: ", cards.Count);  // Verifique o número de cartas após carregar
    }

    // Método para comprar uma carta
    public Card DrawCard()
    {

        GD.Print("Tentando comprar carta. Cartas no deck: ", cards.Count);

        if (cards.Count == 0)
        {
            GD.Print("O deck está vazio!");
            return null;
        }

        Card drawnCard = cards[0];
        cards.RemoveAt(0);


        if (drawnCard == null)
        {
            GD.PrintErr("Erro: Carta retirada do deck é nula.");
            return null;
        }

        // Verifica o board
        if (board == null)
        {
            GD.PrintErr("Erro: Board não encontrado.");
            return null;
        }


        board.AddCardToHand(drawnCard);

        return drawnCard;
    }

    // Método para embaralhar o deck
    public void Shuffle()
    {
        // Verifica se o AnimationPlayer está disponível
        /* if (animationPlayer != null)
         {
             // Inicia a animação de embaralhar
             animationPlayer.Play("shuffle_animation");
         }*/

        // Cria uma instância de Random para gerar números aleatórios
        Random rand = new Random();

        // Embaralha a lista de cartas usando o algoritmo Fisher-Yates
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1); // Gera um índice aleatório entre 0 e n
            Card value = cards[k];    // Obtém a carta no índice aleatório
            cards[k] = cards[n];      // Troca a carta no índice aleatório com a carta no índice n
            cards[n] = value;         // Coloca a carta no índice n
        }
    }
}
