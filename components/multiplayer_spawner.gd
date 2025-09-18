extends MultiplayerSpawner


@export var scene_to_spawn : PackedScene
@export var player_1_spawn_point : Marker3D
@export var player_2_spawn_point : Marker3D


func _ready():
	multiplayer.peer_connected.connect(spawn_player)


func spawn_player(id: int):
	if !multiplayer.is_server(): return
	print("connected: " + str(id))
	print("peers: " + str(multiplayer.get_peers()))
	var peer_count := multiplayer.get_peers().size()

	var spawn_point : Marker3D 
	if peer_count==1:
		spawn_point = player_1_spawn_point
	elif peer_count==2:
		spawn_point = player_2_spawn_point
	else:
		return

	var player : Player = scene_to_spawn.instantiate()
	player.name = str(id)
	player.position = spawn_point.position
	player.setup_multiplayer(id)
	spawn_point.get_parent().call_deferred("add_child", player)


	
