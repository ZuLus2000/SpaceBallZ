class_name Player extends RigidBody3D



@onready var player_camera : Camera3D = %PlayerCamera

var default_coordinates : Vector3

func _ready():
	assert(player_camera)
	# Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	player_camera.current = is_multiplayer_authority()
	pass

# @rpc("call_local")
func setup_multiplayer(id: int):
	set_multiplayer_authority(id)
	pass

func _enter_tree() -> void:
	set_multiplayer_authority(name.to_int())
	position = default_coordinates
