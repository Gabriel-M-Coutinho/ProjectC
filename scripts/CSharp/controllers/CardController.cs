using Godot;
using System;

public partial class CardController : Node2D
{
    private RayCast2D _raycast;
    private Card _currentHoveredCard = null; // Armazena a carta atualmente destacada

    public override void _Ready()
    {
        _raycast = GetNode<RayCast2D>("RayCast2D");
        _raycast.TargetPosition = new Vector2(0, 0); // Começa sem alvo específico
        _raycast.Enabled = true;
    }

    public override void _Process(double delta)
    {
        UpdateRaycast();
    }

    private void UpdateRaycast()
    {
        // Atualiza a posição do RayCast2D para a posição do mouse
        Vector2 mousePos = GetGlobalMousePosition();
        _raycast.GlobalPosition = mousePos;

        // Checa se o RayCast2D colidiu com algo
        if (_raycast.IsColliding())
        {
            GodotObject hitObject = _raycast.GetCollider(); // Retorna um GodotObject

            if (hitObject is Node node) // Converte primeiro para Node
            {
                if (node is Card card) // Agora verifica se é um Card
                {
                    if (_currentHoveredCard != card)
                    {
                        _currentHoveredCard?.OnHoverChanged(false);
                        _currentHoveredCard = card;
                        _currentHoveredCard.OnHoverChanged(true);
                    }
                    return;
                }
            }
        }

        // Se não estiver sobre nenhuma carta, reseta o hover
        if (_currentHoveredCard != null)
        {
            _currentHoveredCard.OnHoverChanged(false);
            _currentHoveredCard = null;
        }
    }

    // Adicionando o método que retorna a carta sob o mouse
    public Card CheckForCardAtMousePosition()
    {
        // Atualiza a posição do RayCast2D para a posição do mouse
        Vector2 mousePos = GetGlobalMousePosition();
        _raycast.GlobalPosition = mousePos;

        // Checa se o RayCast2D colidiu com algo
        if (_raycast.IsColliding())
        {
            GodotObject hitObject = _raycast.GetCollider(); // Retorna um GodotObject

            if (hitObject is Node node) // Converte primeiro para Node
            {
                if (node is Card card) // Agora verifica se é um Card
                {
                    return card; // Retorna a carta se houver colisão
                }
            }
        }

        return null; // Retorna null se não houver colisão
    }
}
