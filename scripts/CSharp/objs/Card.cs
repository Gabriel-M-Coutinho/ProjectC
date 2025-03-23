using Godot;
using System.Collections.Generic;

public partial class Card : Control
{
    //region SINAIS E PROPRIEDADES
    [Signal]
    public delegate void OpenStatusEventHandler(Card card);

    [Export]
    private float _scaleCard = 1.0f;

    private Sprite2D _sprite;
    private Area2D _area2D;
    private CollisionShape2D _collisionShape;
    private bool _isHovering = false;

    // Enum para definir os locais possíveis da carta
    public enum Location
    {
        Deck,
        Hand,
        Field,
        Graveyard
    }

    // Propriedade para armazenar a localização atual da carta

    public Location CurrentLocation { get; set; } = Location.Deck;

    // ESTADO
    public int CardId { get; set; }
    public int PlayerId { get; set; }
    public Dictionary<string, object> CardData { get; set; }
    private Vector2 _originalScale;
    private Vector2 _targetScale;

    // CICLO DE VIDA
    public override void _Ready()
    {


        // Tente obter o sprite
        _sprite = GetNode<Sprite2D>("CardImage");
        GD.Print($"_sprite após GetNode: {(_sprite == null ? "NULL" : "OK")}");

        // Resto do código...
        _area2D = GetNode<Area2D>("Area2D");
        _collisionShape = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");


        _originalScale = Scale;
        _targetScale = _originalScale;

        // Garante que a carta processe eventos de mouse
        MouseFilter = MouseFilterEnum.Stop;

        // Conecta os sinais da Area2D
        _area2D.MouseEntered += OnMouseEntered;
        _area2D.MouseExited += OnMouseExited;
        _area2D.InputEvent += OnAreaInputEvent;


    }

    //region SETUP DINÂMICO
    public void Setup(Dictionary<string, object> cardData, int cardId, int playerId)
    {
        PlayerId = playerId;
        CardId = cardId;

        if (cardData == null)
        {
            GD.PrintErr("Erro: cardData é nulo!");
            return;
        }

        CardData = cardData;

        // Certifique-se de que o sprite está inicializado
        if (_sprite == null)
        {
            _sprite = GetNode<Sprite2D>("CardImage");

        }

        if (CardData.ContainsKey("image_path"))
        {
            string imagePath = (string)CardData["image_path"];


            if (!string.IsNullOrEmpty(imagePath))
            {

                // Verificação detalhada
                _sprite = GetNode<Sprite2D>("CardImage");


                Texture2D image = ResourceLoader.Load<Texture2D>(imagePath);

                if (image != null && _sprite != null)
                {
                    _sprite.Texture = image;
                    GD.Print("Imagem carregada com sucesso: ", imagePath);
                }
                else
                {
                    GD.PrintErr("Falha ao carregar imagem: ", imagePath);
                    if (_sprite == null) GD.PrintErr("_sprite é nulo no momento de definir a textura!");
                }
            }
        }
    }

    public override void _Process(double delta)
    {
        // Interpola a escala da carta
        Scale = Scale.Lerp(_targetScale, 0.1f);
    }

    //region INTERAÇÃO COM O MOUSE
    private void OnMouseEntered()
    {
        _isHovering = true;
        OnHoverChanged(true);
    }

    private void OnMouseExited()
    {
        _isHovering = false;
        OnHoverChanged(false);
    }

    private void OnAreaInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                GD.Print("Clique na carta: ", CardData.GetValueOrDefault("name", "Sem Nome"));
                AbrirCardUI();
            }
        }
    }
    public void OnHoverChanged(bool isHovering)
    {
        if (isHovering)
        {
            _targetScale = _originalScale * 1.05f; // Aumenta a escala da carta
            GD.Print("Mouse sobre a carta: ", CardData.GetValueOrDefault("name", "Sem Nome"));
        }
        else
        {
            _targetScale = _originalScale; // Retorna à escala original
            GD.Print("Mouse saiu da carta: ", CardData.GetValueOrDefault("name", "Sem Nome"));
        }
    }
    //endregion

    //region UI DA CARTA
    private void AbrirCardUI()
    {
        Node cardUI = GetTree().CurrentScene.FindChild("CardInfoUI", true, false);
        if (cardUI != null)
        {
            GD.Print("Abrindo UI para carta: ", CardData.GetValueOrDefault("name", "Sem Nome"));
            GD.Print("Dados da carta: ", CardData);

            // Converte para Godot.Collections.Dictionary
            var godotCardData = new Godot.Collections.Dictionary<string, Variant>();
            foreach (var kvp in CardData)
            {
                godotCardData[kvp.Key] = Variant.From(kvp.Value);
            }

            cardUI.Call("UpdateCardUI", godotCardData, this);
        }
        else
        {
            GD.Print("Erro: CardInfoUI não encontrada!");
        }
    }
    //endregion
}