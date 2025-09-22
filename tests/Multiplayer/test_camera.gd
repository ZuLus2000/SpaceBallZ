extends GutTest


var default_scene : PackedScene = load('res://scenes/default_scene_3d.tscn')

func before_all():
	NetworkHandler.create_server()

func test_camera():
	var root_instance = default_scene.instantiate()
	get_tree().root.call_deferred("add_child", root_instance)
	NetworkHandler.create_client()
	var manager : Manager = root_instance.get_node("Manager")
	assert_gt(manager.connected_players.size(), 0)
	# assert_true(manager.connected_players[0].player_camera.current)
