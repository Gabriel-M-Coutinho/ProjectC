using Godot;

public partial class Board : Control
{
	private GameController gameController;
	private CardController cardDetector;

	private HBoxContainer handContainer;


	public override void _Ready()
	{
		// Acessando o GameController (caminho absoluto)
		gameController = GetTree().Root.GetNode<GameController>("Main/GameController");

		// Acessando o CardController dentro do Board (caminho relativo)
		cardDetector = GetNode<CardController>("CardController");

		// Acessando o HBoxContainer dentro do MarginContainer (caminho relativo)
		handContainer = GetNode<HBoxContainer>("MarginContainer/HBoxContainer");

		handContainer.AddThemeConstantOverride("separation", 170); // Ajuste este valor conforme necessário



	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left)
			{
				// Detecta a carta sob o mouse através do CardController
				Card card = cardDetector.CheckForCardAtMousePosition();
				if (card != null)
				{
					// Processa o clique na carta
					HandleCardClick(card);
				}
			}
		}
	}

	private void HandleCardClick(Card card)
	{
		//GD.Print("Carta clicada: ", card.CardData["name"]);

		// Verifica a localização atual da carta e decide para onde movê-la
		switch (card.CurrentLocation)
		{
			case Card.Location.Hand:
				// Move da mão para o campo (apenas para o jogador 1, por enquanto)
				gameController.MoveCardToField(card, 1);
				break;

			case Card.Location.Field:
				// Move do campo para o cemitério
				gameController.MoveCardToGraveyard(card, 1);
				break;

				// Adicione mais casos conforme necessário
		}
	}

	public void AddCardToHand(Card card)
	{
		GD.Print("Adicionando carta à mão: ", card.CardData["name"]);

		if (card != null && handContainer != null)
		{
			// Garantir que a carta seja visível no contêiner
			card.Position = Vector2.Zero;  // Ou a posição desejada no contêiner
			handContainer.AddChild(card);
		}
		else
		{
			GD.PrintErr("Erro: Não foi possível adicionar a carta à mão.");
		}
	}
}
