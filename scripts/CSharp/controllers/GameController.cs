using Godot;
using System;
using System.Collections.Generic;

public partial class GameController : Node
{
	// Referências para as áreas do jogo
	public Deck Player1Deck { get; private set; }
	public Deck Player2Deck { get; private set; }
	public Graveyard Player1Grave { get; private set; }
	public Graveyard Player2Grave { get; private set; }
	public Hand Player1Hand { get; private set; }
	public Hand Player2Hand { get; private set; }

	private int player1Life = 8000;
	private int player2Life = 8000;
	private int currentTurn = 1;

	private List<Dictionary<string, object>> exampleDeckData = new List<Dictionary<string, object>>
	{
		new Dictionary<string, object>
		{
			{ "name", "Dragão " },
			{ "description", "Um dragão poderoso com escamas brancas." },
			{ "image_path", "res://cards/pegrande.png" },
			{ "atk", 12 },
			{ "life", 15 }
		},
		new Dictionary<string, object>
		{
			{ "name", "Dragão Branco" },
			{ "description", "Um dragão poderoso com escamas brancas." },
			{ "image_path", "res://cards/pegrande.png" },
			{ "atk", 12 },
			{ "life", 15 }
		},
		new Dictionary<string, object>
		{
			{ "name", "Dragão Branco" },
			{ "description", "Um dragão poderoso com escamas brancas." },
			{ "image_path", "res://cards/cerberusr.png" },
			{ "atk", 12 },
			{ "life", 15 }
		}
	};

	public override void _Ready()
	{

		Player1Deck = GetNode<Deck>("../Board/DeckP1");
		Player2Deck = GetNode<Deck>("../Board/DeckP2");
		Player1Grave = GetNode<Graveyard>("../Board/GraveyardP1");
		Player2Grave = GetNode<Graveyard>("../Board/GraveyardP2");
		Player1Hand = GetNode<Hand>("../Board/HandP1");
		Player2Hand = GetNode<Hand>("../Board/HandP2");

		// Carrega e embaralha o deck do jogador 1
		Player1Deck.LoadDeck(exampleDeckData, 1);
		//Player1Deck.Shuffle();


		// Compra cartas iniciais
		for (int i = 0; i < 3; i++)
		{
			DrawCard(1);
		}
	}

	public void DrawCard(int player)
	{

		if (player == 1)
		{
			Card drawnCard = Player1Deck.DrawCard();
			if (drawnCard != null)
			{
				drawnCard.CurrentLocation = Card.Location.Hand;
				Player1Hand.AddCard(drawnCard);  // Mantém o controle lógico da carta

				// Remova esta linha ou a modifique para não adicionar como filho novamente
				// board.AddCardToHand(drawnCard);
			}
		}
		else if (player == 2)
		{
			Card drawnCard = Player2Deck.DrawCard();
			if (drawnCard != null)
			{
				drawnCard.CurrentLocation = Card.Location.Hand;
				Player2Hand.AddCard(drawnCard);
				GD.Print("Jogador 2 comprou: ", drawnCard.CardData["name"]);

			}
		}
	}

	public void MoveCardToField(Card card, int player)
	{
		if (player == 1)
		{
			Player1Hand.RemoveCard(card);
			card.CurrentLocation = Card.Location.Field;
			GD.Print("Carta movida para o campo: ", card.CardData["name"]);
		}
		else if (player == 2)
		{
			Player2Hand.RemoveCard(card);
			card.CurrentLocation = Card.Location.Field;
			GD.Print("Carta movida para o campo: ", card.CardData["name"]);
		}
	}

	public void MoveCardToGraveyard(Card card, int player)
	{
		if (player == 1)
		{
			Player1Grave.AddToGrave(card);
			GD.Print("Carta movida para o cemitério: ", card.CardData["name"]);
		}
		else if (player == 2)
		{
			Player2Grave.AddToGrave(card);
			GD.Print("Carta movida para o cemitério: ", card.CardData["name"]);
		}
	}
}
