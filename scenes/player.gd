class_name Player extends RigidBody3D


@export var move_speed : float = 0
@export var acceleration : float = 0
@export var use_floating_physics : bool = true

@onready var _player_camera : Camera3D = %PlayerCamera


var default_coordinates : Vector3

var desired_direction := Vector3.ZERO

var _movement_baseline : Basis


func _ready():
	assert(_player_camera)

func _enter_tree() -> void:
	set_multiplayer_authority(name.to_int())
	position = default_coordinates

func _apply_input_force(force: Vector3) -> void:
	apply_central_force(force)

func _physics_process(_delta: float) -> void:
	if !is_multiplayer_authority(): return
	var move_direction := desired_direction
	if use_floating_physics: 
		_apply_input_force(move_direction*move_speed)
		return
	if move_direction:
		var _kc3d := move_and_collide(move_direction * move_speed)


func setup_camera(state: bool) -> void:
	_player_camera.current = state
	pass
