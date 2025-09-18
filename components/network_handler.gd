extends Node


@export var ip_address : String = "127.0.0.1"
@export var port : int = 53135


var peer : ENetMultiplayerPeer


func create_server() -> void:
	peer = ENetMultiplayerPeer.new()
	peer.create_server(port)
	multiplayer.multiplayer_peer = peer

func create_client() -> void:
	peer = ENetMultiplayerPeer.new()
	peer.create_client(ip_address, port)
	multiplayer.multiplayer_peer = peer
	
