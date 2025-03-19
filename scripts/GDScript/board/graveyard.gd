extends Node

class_name Graveyard;

var cards: Array = []

func add_to_grave(card):
	cards.append(card)
	print("Carta enviada ao cemitério:", card)

func retrieve_from_grave(index: int):
	if index >= 0 and index < cards.size():
		return cards.pop_at(index)  # Remove e retorna a carta escolhida
	return null

func clear_grave():
	cards.clear()
