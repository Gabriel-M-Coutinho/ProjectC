extends Node2D
@onready var carInfoUI = $CardInfoUI
@onready var hoboxHand = $HBoxContainer

var player1_deck: Deck
var player2_deck: Deck

var player1_grave: Graveyard
var player2_grave: Graveyard 

var player1_hand: Hand
var player2_hand: Hand 

var player1_life: int = 8000
var player2_life: int = 8000

var current_turn: int = 1 

var example_deck_data:Array[Dictionary] = [
	{
		"name": "Dragão ",
		"description": "Um dragão poderoso com escamas brancas.",
		"image_path": "res://cards/cerberusr.png",
		"atk": 12,
		"life": 15
	},
	{
		"name": "ragão Branco",
		"description": "Um dragão poderoso com escamas brancas.",
		"image_path": "res://cards/cerberusr.png",
		"atk": 12,
		"life": 15
	},
		{
		"name": "Do Branco",
		"description": "Um dragão poderoso com escamas brancas.",
		"image_path": "res://cards/cerberusr.png",
		"atk": 12,
		"life": 15
	},
			{
		"name": " Branco",
		"description": "Um dragão poderoso com escamas brancas.",
		"image_path": "res://cards/cerberusr.png",
		"atk": 12,
		"life": 15
	},		{
		"name": "Dragão Branco",
		"description": "Um dragão poderoso com escamas brancas.",
		"image_path": "res://cards/cerberusr.png",
		"atk": 12,
		"life": 15
	},

]

func _ready() -> void:
	player1_deck = Deck.new()
	player1_hand = Hand.new()
	player1_deck.load_deck(example_deck_data,1)
	player1_deck.shuffle()
	self.draw_card(1)
	self.draw_card(1)
	self.draw_card(1)
	self.draw_card(1)
	self.draw_card(1)



func setup_game(p1_deck_data: Array[Dictionary], p2_deck_data: Array[Dictionary]):
	player1_deck = Deck.new()
	player1_deck.load_deck(p1_deck_data,1)
	
	player2_deck = Deck.new()
	player2_deck.load_deck(p2_deck_data,2)
	
	player1_hand = Hand.new()
	player2_hand = Hand.new()
	
	player1_grave = Graveyard.new()
	player2_grave = Graveyard.new()

	player1_life = 8000
	player2_life = 8000

	#start_turn()

func start_turn():
	print("Vez do jogador:", current_turn)

func end_turn():
	current_turn = 2 if current_turn == 1 else 1
	start_turn()

func draw_card(player: int):
	if player == 1:
		var drawn_card:Card = player1_deck.draw_card()
		if drawn_card:
			drawn_card.current_location = Card.Location.HAND
			player1_hand.add_card(drawn_card)
			hoboxHand.add_child(drawn_card)
			


	elif player == 2:
		var drawn_card = player2_deck.draw_card()
		if drawn_card:
			player2_hand.add_card(drawn_card)
			print("Jogador 2 comprou:", drawn_card.get_card_name())
