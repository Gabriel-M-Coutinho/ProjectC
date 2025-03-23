extends Control
class_name Card

#region SINAIS E PROPRIEDADES
# Sinal emitido quando o status da carta é alterado (por exemplo, quando é virada)
signal open_status(card)

# Propriedades exportadas para ajustes no editor
@export var scalecard: float = 0.09

# Referências aos nós da cena
@onready var sprite = $CardImage
@onready var cardShape = $CardShape
@onready var collisionShape = $Area2D/CollisionShape2D
#endregion

#region VARIÁVEIS DE ESTADO
# Variáveis de estado da carta
var arr_idx: int
var player: int
var cardId: int
var card_data: Dictionary
var card_atk: int
var card_life: int
var is_face_down: bool
var current_location: Location = Location.DECK # Localização inicial da carta (DECK)
var originalScale: Vector2
var targetScale: Vector2

# Variáveis para controle de hover
var is_hovering: bool = false
var hover_priority: int = 0 # Quanto maior, mais prioritário
#endregion

#region ENUMS
# Enum para controlar a posição da carta
enum Location {
	DECK,
	GRAVEYARD,
	HAND
}
#endregion

#region CONFIGURAÇÃO INICIAL
# Método para configurar a carta com informações iniciais
func setup(card_info: Dictionary, id: int, player_id: int):
	self.cardId = id
	self.player = player_id
	self.card_data = card_info
	self.card_atk = card_info.get("atk")
	self.card_life = card_info.get("life")
	self.current_location = Location.DECK # Definir a localização inicial como DECK
	return self
#endregion

#region VISUAIS
# Método para aplicar as configurações visuais da carta (textura, escala, etc.)
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
#endregion

#region GERENCIAMENTO DE LOCALIZAÇÃO
# Método para alterar a localização da carta (DECK, GRAVEYARD, HAND)
func set_location(new_location: Location, new_index: int = -1):
	current_location = new_location
	if new_index >= 0:
		arr_idx = new_index
	print("Localização alterada")
#endregion

#region FUNÇÕES DO CICLO DE VIDA
# Método chamado quando o nó está pronto
func _ready():
	add_to_group("cards")
	originalScale = scale
	targetScale = originalScale # Inicializa targetScale com o valor original
	
	# Conecta sinais de entrada do mouse
	$Area2D.mouse_entered.connect(_on_mouse_entered)
	$Area2D.mouse_exited.connect(_on_mouse_exited)
	$Area2D.input_event.connect(_on_area_input_event)
	
	custom_minimum_size = Vector2(100, 150)
	apply_visual_settings()

# Método chamado a cada frame para interpolar a escala da carta
func _process(delta):
	scale = scale.lerp(targetScale, 0.1)
#endregion

#region INTERAÇÃO COM O MOUSE
# Método chamado quando há um evento de input na área da carta
func _on_area_input_event(_area, event, _shape_idx):
	if event is InputEventMouseButton and event.pressed:
		# Só processa o clique se esta carta tiver o hover ativo
		if is_hovering and hover_priority == get_highest_hover_priority():
			if event.button_index == MOUSE_BUTTON_LEFT:
				print("Clique na carta: ", card_data.get("name", "Sem Nome"))
				abrir_card_ui()
			elif event.button_index == MOUSE_BUTTON_RIGHT and (player == 1 or current_location == Location.HAND):
				abrir_card_ui()

# Métodos para gerenciar o hover da carta
func _on_mouse_entered():
	is_hovering = true
	hover_priority = get_hover_priority()
	check_hover_status()

func _on_mouse_exited():
	is_hovering = false
	hover_priority = 0
	targetScale = originalScale
	for card in get_tree().get_nodes_in_group("cards"):
		if card != self and card.is_hovering:
			card.check_hover_status()

# Método para verificar se a carta deve ter o hover aplicado
func check_hover_status():
	var highest_priority_card = self
	var highest_priority = hover_priority
	
	for card in get_tree().get_nodes_in_group("cards"):
		if card != self and card.is_hovering and card.hover_priority > highest_priority:
			highest_priority = card.hover_priority
			highest_priority_card = card
	
	if highest_priority_card == self:
		targetScale = originalScale * 1.05
	else:
		targetScale = originalScale

# Método para obter a prioridade do hover (baseada na posição Y)
func get_hover_priority():
	return -global_position.y # Negativo porque menor Y = mais no topo da tela

# Método para obter a maior prioridade entre todas as cartas
func get_highest_hover_priority() -> int:
	var highest_priority = hover_priority
	
	for card in get_tree().get_nodes_in_group("cards"):
		if card != self and card.is_hovering and card.hover_priority > highest_priority:
			highest_priority = card.hover_priority
	
	return highest_priority
#endregion

#region UI DA CARTA
# Método para abrir a UI da carta
func abrir_card_ui():
	var card_ui = get_node("/root/Board/CardInfoUI") # Caminho da sua UI
	if card_ui:
		print("Abrindo UI para carta: ", card_data.get("name", "Sem Nome"))
		print("Dados da carta: ", card_data)
		card_ui.update_card_ui(card_data, self)
	else:
		print("CardInfoUI não encontrada!")
#endregion
