extends Node


@onready var controlled_body: Player = get_parent()


func _physics_process(_delta: float) -> void:
	var movement_input := Input.get_vector( "Left", "Right", "Down", "Up",)
	controlled_body.desired_direction = Vector3(movement_input.x, movement_input.y, 0.0)
