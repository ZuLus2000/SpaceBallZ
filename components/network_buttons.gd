extends Node


func _on_server_btn_pressed() -> void:
	NetworkHandler.create_server()

func _on_client_btn_pressed() -> void:
	NetworkHandler.create_client()
