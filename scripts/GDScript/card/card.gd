extends Control
class_name Card
signal open_status(card)
@onready var sprite = $CardImage
@onready var cardShape = $CardShape
@onready var collisionShape = $Area2D/CollisionShape2D
@export var scalecard: float = 0.09
var arr_idx: int
var player: int
var cardId: int
var card_data: Dictionary
var card_atk: int
var card_life: int
var is_face_down: bool
var current_location: Location = Location.DECK  # Localização inicial da carta (DECK)
var originalScale: Vector2
var targetScale: Vector2
# Variáveis para controle de hover
var is_hovering: bool = false
var hover_priority: int = 0  # Quanto maior, mais prioritário

# Enum Location para controlar a posição da carta
enum Location {
	DECK,
	GRAVEYARD,
	HAND
}

func setup(card_info: Dictionary, id: int, player_id: int):
	self.cardId = id
	self.player = player_id
	self.card_data = card_info
	self.card_atk = card_info.get("atk")
	self.card_life = card_info.get("life")
	self.current_location = Location.DECK  # Definir a localização inicial como DECK
	return self

func apply_visual_settings():
	if card_data:
		var image_path = card_data.get("image_path", "res://cards/cerberusr.png")
		if image_path:
			var image = load(image_path)
			if image and sprite:
				sprite.texture = image
				var texture_size = sprite.texture.get_size()
				var desired_width = 2000
				var desired_height = 2500
				var scale_x = desired_width / texture_size.x
				var scale_y = desired_height / texture_size.y
				var final_scale = min(scale_x, scale_y) * scalecard
				sprite.scale = Vector2(final_scale, final_scale)
				if cardShape:
					cardShape.scale = sprite.scale
				if collisionShape and collisionShape.shape is RectangleShape2D:
					var shape = collisionShape.shape
					shape.extents = Vector2(desired_width, desired_height) * final_scale * 0.5
			else:
				push_error("Sprite não encontrado na carta")

func set_location(new_location: Location, new_index: int = -1):
	current_location = new_location
	if new_index >= 0:
		arr_idx = new_index
	print("Localização alterada")

func _ready():
	add_to_group("cards")
	originalScale = scale
	targetScale = originalScale  # Inicializa targetScale com o valor original
	
	$Area2D.mouse_entered.connect(_on_mouse_entered)
	$Area2D.mouse_exited.connect(_on_mouse_exited)
	$Area2D.input_event.connect(_on_area_input_event)
	
	custom_minimum_size = Vector2(100, 150)
	apply_visual_settings()

func _process(delta):
	# Interpola a escala gradualmente em direção ao targetScale
	scale = scale.lerp(targetScale, 0.1)

func _on_area_input_event(_area, event, _shape_idx):
	if event is InputEventMouseButton and event.pressed:
		# Só processa o clique se esta carta tiver o hover ativo
		if is_hovering and hover_priority == get_highest_hover_priority():
			if event.button_index == MOUSE_BUTTON_LEFT:
				print("Clique na carta: ", card_data.get("name", "Sem Nome"))
				abrir_card_ui()
			elif event.button_index == MOUSE_BUTTON_RIGHT and (player == 1 or current_location == Location.HAND):
				abrir_card_ui()

# Função para abrir a UI do card
func abrir_card_ui():
	# Acesse a UI diretamente e passe os dados da carta
	var card_ui = get_node("/root/Board/CardInfoUI")  # Caminho da sua UI
	if card_ui:
		print("Abrindo UI para carta: ", card_data.get("name", "Sem Nome"))
		print("Dados da carta: ", card_data)
		card_ui.update_card_ui(card_data, self)
	else:
		print("CardInfoUI não encontrada!")

# Modificamos estas funções para gerenciar o hover com prioridade
func _on_mouse_entered():
	is_hovering = true
	
	# Quando o mouse entra, aumenta a prioridade desta carta
	hover_priority = get_hover_priority()
	
	# Verifica se esta carta deve ter o hover ativo
	check_hover_status()

func _on_mouse_exited():
	is_hovering = false
	hover_priority = 0
	
	# Restaura a escala
	targetScale = originalScale
	
	# Informa outras cartas para verificarem seu status
	for card in get_tree().get_nodes_in_group("cards"):
		if card != self and card.is_hovering:
			card.check_hover_status()

# Nova função para verificar se esta carta deve ter o hover aplicado
func check_hover_status():
	# Encontra a carta com maior prioridade
	var highest_priority_card = self
	var highest_priority = hover_priority
	
	for card in get_tree().get_nodes_in_group("cards"):
		if card != self and card.is_hovering and card.hover_priority > highest_priority:
			highest_priority = card.hover_priority
			highest_priority_card = card
	
	# Aplica o hover apenas na carta com maior prioridade
	if highest_priority_card == self:
		targetScale = originalScale * 1.05
	else:
		targetScale = originalScale

# Define a prioridade do hover (normalmente baseada na ordem Z)
func get_hover_priority():
	# A prioridade pode ser baseada na posição Y (quanto menor Y, mais para o topo visualmente)
	# Ou você pode usar a ordem Z da carta se estiver definida
	return -global_position.y  # Negativo porque menor Y = mais no topo da tela

# Função auxiliar para obter a maior prioridade entre todas as cartas
func get_highest_hover_priority() -> int:
	var highest_priority = hover_priority
	
	for card in get_tree().get_nodes_in_group("cards"):
		if card != self and card.is_hovering and card.hover_priority > highest_priority:
			highest_priority = card.hover_priority
	
	return highest_priority
