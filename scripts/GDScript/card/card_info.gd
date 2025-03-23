# Na classe CardInfoUI:
extends Node2D

#region REFERÊNCIAS A NÓS
@onready var button = $Control/Button
@onready var descriptionText = $Control/DescriptionLabel
@onready var nameText = $Control/NameLabel
@onready var cardImage = $Control/SpriteArt
@onready var atk = $Control/ATK
@onready var life = $Control/LIFE
#endregion

#region VARIÁVEIS DE ESTADO
var current_card = null  # Para rastrear qual carta abriu a UI

#endregion

#region CICLO DE VIDA
func _ready() -> void:
	visible = false  # Garante que a UI esteja oculta inicialmente
	button.pressed.connect(on_close_button_pressed)  # Conecta o botão de fechar
#endregion

#region INTERAÇÃO COM O BOTÃO
# Método chamado quando o botão de fechar é pressionado
func on_close_button_pressed() -> void:
	visible = false
	current_card = null
#endregion

#region ATUALIZAÇÃO DA UI
# Método para atualizar a UI com os dados da carta
func update_card_ui(card_data: Dictionary, source_card = null) -> void:
	current_card = source_card
	
	print("Atualizando UI com carta: ", card_data.get("name", "Sem Nome"))
	
	# Atualiza os textos e a imagem da carta
	nameText.text = card_data.get("name", "Sem Nome")
	descriptionText.text = card_data.get("description", "Sem Descrição")
	atk.text = str(card_data.get("atk", 0))
	life.text = str(card_data.get("life", 0))
	
	# Carrega e exibe a imagem da carta
	var image_path = card_data.get("image_path", "res://default_card_image.png")
	var texture = load(image_path)
	if texture: 
		cardImage.texture = texture
		cardImage.scale = Vector2(0.25, 0.25)
	
	visible = true  # Torna a UI visível
#endregion
