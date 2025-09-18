extends Node

@export var move_speed : float = 0
@export var acceleration : float = 0
@export var use_floating_physics : bool = true

@export_range(0,10) var mouse_sence := 0.0


@onready var _controlled_body : RigidBody3D = self.get_parent()


func _ready():
	# Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	pass


func _unhandled_input(_event: InputEvent) -> void:
	pass
	

func _apply_input_force(force: Vector3) -> void:
	_controlled_body.apply_central_force(force)
	pass

func _process_move(_delta: float) -> void:
	if !_controlled_body.is_multiplayer_authority(): return
	var movement_input := Input.get_vector("Down", "Up", "Left", "Right")
	var move_direction := Vector3(movement_input.y, movement_input.x, 0.0)
	if use_floating_physics: 
		_apply_input_force(move_direction*move_speed)
		return
	if move_direction:
		var _kc3d := _controlled_body.move_and_collide(move_direction * move_speed)


func _physics_process(delta: float) -> void:
	_process_move(delta)
