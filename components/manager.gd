class_name Manager extends Node


enum Team {team_1, team_2}

static var manager_instance : Manager = self

@export var arena: Arena
@export var shoot_point : ShootPoint

@export var team_1_score_label : Label
@export var team_2_score_label : Label

@export var player_spawner : PlayerSpawner

var controlled_player : Player

var scoring_ball: Ball

var _score_team_1 : int = 0
var _score_team_2 : int = 0

func _check_if_all_values_set() -> void:
	for value in [arena, shoot_point, team_1_score_label, team_2_score_label]:
		assert(value != null)



func _ready():
	if manager_instance == null: manager_instance = self
	_check_if_all_values_set()
	player_spawner.player_spawned.connect(_set_player_camera)
	arena.team_score.connect(scored)
	shoot_point.ball_spawned.connect(_set_scoring_ball)
	shoot_point.spawn_ball()

func _update_scores() -> void:
	team_1_score_label.text = str(_score_team_1)
	team_2_score_label.text = str(_score_team_2)

func _change_score(team_id: Team, _score: int) -> void:
	if team_id == Team.team_1:
		_score_team_1 += _score
	elif team_id == Team.team_2:
		_score_team_2 += _score
	_update_scores()


func scored(team_id: int) -> void:
	if team_id == 1:
		print("team 1 scored!")
		_change_score(Team.team_1, 1)
	elif team_id == 2:
		print("team 2 scored!")
		_change_score(Team.team_2, 1)
	scoring_ball.queue_free()
	shoot_point.spawn_ball()	

func _set_scoring_ball(ball: Ball) -> void:
	scoring_ball = ball

func _set_player_camera(player_instance: Player) -> void:
	await player_instance.ready
	if multiplayer.is_server():
		return
	var is_spawned_player_under_my_control := multiplayer.multiplayer_peer.get_unique_id() == player_instance.get_multiplayer_authority()
	if controlled_player == null and is_spawned_player_under_my_control:
			controlled_player = player_instance
			player_instance.setup_camera(true)
	
	
func bounced(_body) -> void:
	print("Bounced!")
