class_name Player extends RigidBody3D



@onready var _player_camera : Camera3D = %PlayerCamera


var default_coordinates : Vector3


func _ready():
	assert(_player_camera)

func _enter_tree() -> void:
	set_multiplayer_authority(name.to_int())
	position = default_coordinates



func setup_camera(state: bool) -> void:
	_player_camera.current = state
	pass
