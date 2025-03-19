extends Node

class_name Deck

var CardScene = preload("res://scenes/card.tscn")

var cards: Array[Card] = []
var next_card_id: int = 0  # Contador para gerar IDs únicos

func load_deck(deck_data: Array[Dictionary], player_id: int):
	for card_data in deck_data:
		var new_card: Card = CardScene.instantiate() as Card
		if new_card:
			# Gera um ID único e passa para o setup da carta
			new_card.setup(card_data, next_card_id, player_id)
			cards.append(new_card)
			next_card_id += 1  # Incrementa o ID para a próxima carta

func draw_card() -> Card:
	if cards.is_empty():
		print("O deck está vazio!")
		return null
	return cards.pop_front()

func shuffle():
	cards.shuffle()
