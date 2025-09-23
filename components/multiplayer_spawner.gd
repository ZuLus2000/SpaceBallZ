class_name PlayerSpawner extends MultiplayerSpawner


@export var scene_to_spawn : PackedScene
@export var player_1_spawn_point : Marker3D
@export var player_2_spawn_point : Marker3D

signal player_spawned(player_instance: Player)


func _spawn_function(data: Dictionary) -> Node:
	var player : Player = scene_to_spawn.instantiate()
	player.name = str(data.id)
	player.default_coordinates = data.default_coordinates
	if data.invert_direction:
		player.rotate_y(PI)
	player_spawned.emit(player)
	return player


func _ready():
	spawn_function = _spawn_function
	multiplayer.peer_connected.connect(spawn_player)


func spawn_player(id: int):
	if !multiplayer.is_server(): return
	print("connected: " + str(id))
	print("peers: " + str(multiplayer.get_peers()))
	var peer_count := multiplayer.get_peers().size()

	var spawn_point : Marker3D 
	var invert_direction : bool
	if peer_count==1:
		spawn_point = player_1_spawn_point
		invert_direction = true
	elif peer_count==2:
		spawn_point = player_2_spawn_point
		invert_direction = false
	else:
		return

	var data : Dictionary  = {
		id = id,
		default_coordinates = spawn_point.position,
		invert_direction = invert_direction
		}

	
	spawn(data)
	# spawn_point.get_parent().call_deferred("add_child", spawn(data))
	
	



	
