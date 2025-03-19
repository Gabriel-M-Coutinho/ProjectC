extends Object  # Opcional, pode ser removido

class_name FileUtils  


static func save_data(content, path: String) -> bool:
	var file = FileAccess.open(path, FileAccess.WRITE)
	if file:
		file.store_var(content) 
		file.close()
		print("Dados salvos com sucesso em:", path)
		return true
	else:
		print("Erro ao abrir o arquivo para escrita:", path)
		return false

# Carrega dados de um arquivo binário usando get_var
static func load_data(path: String):
	var file = FileAccess.open(path, FileAccess.READ)
	if file:
		var content = file.get_var()  # Carrega o conteúdo salvo
		file.close()
		print("Dados carregados com sucesso de:", path)
		return content
	else:
		print("Erro ao abrir o arquivo para leitura:", path)
		return null
