using Godot;
using System;
using System.Collections.Generic;

public partial class CardInfoModal : Node2D
{
    private Button _button;
    private RichTextLabel _descriptionText;
    private Label _nameText;
    private Sprite2D _cardImage;
    private Label _atk;
    private Label _life;

    private Card _currentCard = null;

    public override void _Ready()
    {
        // Obtenção de nós, ajustando os caminhos se necessário
        _button = GetNode<Button>("Control/Button");
        _descriptionText = GetNode<RichTextLabel>("Control/DescriptionLabel");
        _nameText = GetNode<Label>("Control/NameLabel");
        _cardImage = GetNode<Sprite2D>("Control/SpriteArt");
        _atk = GetNode<Label>("Control/ATK");
        _life = GetNode<Label>("Control/LIFE");

        // Verificação se o botão foi encontrado
        if (_button == null)
        {
            GD.PrintErr("Erro: Botão não encontrado!");
        }
        else
        {
            _button.Pressed += OnCloseButtonPressed;
        }

        // Inicializando o modal invisível
        Visible = false;
    }

    private void OnCloseButtonPressed()
    {
        // Fecha o modal
        Visible = false;
        _currentCard = null;
    }

    public void UpdateCardUI(Godot.Collections.Dictionary<string, Variant> cardData, Card sourceCard = null)
    {
        _currentCard = sourceCard;

        GD.Print("Atualizando UI com carta: ", cardData.GetValueOrDefault("name", "Sem Nome"));

        // Atualiza os textos da interface com as informações da carta
        _nameText.Text = cardData.GetValueOrDefault("name", "Sem Nome").AsString();
        _descriptionText.Text = cardData.GetValueOrDefault("description", "Sem Descrição").AsString();
        _atk.Text = cardData.GetValueOrDefault("atk", 0).AsString();
        _life.Text = cardData.GetValueOrDefault("life", 0).AsString();

        // Carrega e exibe a imagem da carta
        string imagePath = cardData.GetValueOrDefault("image_path", "res://default_card_image.png").AsString();
        var texture = ResourceLoader.Load<Texture2D>(imagePath);

        if (texture != null)
        {
            _cardImage.Texture = texture;
            _cardImage.Scale = new Vector2(0.25f, 0.25f);  // Ajuste o tamanho da imagem conforme necessário
        }
        else
        {
            GD.PrintErr("Erro ao carregar a imagem da carta: ", imagePath);
        }

        // Torna o modal visível
        Visible = true;
    }
}
