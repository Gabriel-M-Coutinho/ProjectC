extends Node
class_name Hand;

var cards: Array = []
const MAX_CARDS = 6  

func add_card(card):
	if cards.size() < MAX_CARDS:
		cards.append(card)
		print("Carta adicionada à mão:", card)
	else:
		print("Mão cheia! Descarte antes de comprar.")

func remove_card(index: int):
	if index >= 0 and index < cards.size():
		return cards.pop_at(index)
	return null

func size():
	return cards.size()

func sort_hand():
	cards.sort() 
