
using Godot;
using System;
using System.Threading.Tasks;

namespace SpaceBallZ
{
    enum Team { Team1, Team2 }

    public partial class Manager : Node
    {

        public static Manager ManagerInstance { get; private set; }
        [Export]
        private AbstractArena _arena;
        [Export]
        private ShootPoint _shootPoint;

        [Export]
        private Label Team1ScoreLabel;
        [Export]
        private Label Team2ScoreLabel;

        [Export]
        private MultiplayerObjectSpawner _playerSpawner;

		[Export]
		private Godot.Collections.Array<BallModifier> _spawnableBuffs = new();

        public bool DebugMode = false;

        public Player ControlledPlayer { get; private set; }

        private Ball _scoringBall;

        private int _scoreTeam1 = 0;
        private int _scoreTeam2 = 0;

        private void CheckIfAllValuesSet()
        {
            Variant[] values = { Team1ScoreLabel, Team2ScoreLabel, _arena, _playerSpawner, _shootPoint };
            for (int i = 0; i < values.Length; i++)
            {
                System.Diagnostics.Debug.Assert(values[i].VariantType != Variant.Type.Nil);

            }
        }

        public override void _Ready()
        {
            CheckIfAllValuesSet();
            ManagerInstance = this;
            // _playerSpawner.Connect(PlayerSpawner.SignalName.PlayerSpawned, Callable.From<Player>(SetPlayerCamera));
            ControlledPlayer = null;
            _playerSpawner.PlayerSpawned += OnPlayerSpawned;
            NetworkHandler.Instance.StateChanged += onStateChanged;
            _shootPoint.BallSpawned += setScoringBall;
        }

        private void onStateChanged(int newState) { if (isServerHost()) _arena.TeamScore += Scored; }
        private void UpdateScores()
        {
            Team1ScoreLabel.Text = _scoreTeam1.ToString();
            Team2ScoreLabel.Text = _scoreTeam2.ToString();
        }

        private void ChangeScores(Team teamId, int score)
        {
            if (teamId == Team.Team1) _scoreTeam1 += score;
            if (teamId == Team.Team2) _scoreTeam2 += score;
            UpdateScores();
        }

        public void Scored(int teamId)
        {
            if (teamId == 1)
            {
                GD.Print("Team 1 scored!");
                ChangeScores(Team.Team1, 1);
            }
            else if (teamId == 2)
            {
                GD.Print("Team 2 scored!"); ChangeScores(Team.Team2, 1);
            }
            _scoringBall.QueueFree();
            _scoringBall = null;
        }

        private void OnPlayerSpawned(Player playerInstance)
        {
            playerInstance.Ready += () => SetPlayerCamera(playerInstance);
        }

        private void SetPlayerCamera(Player playerInstance)
        {
            bool isSpawnedPlayerUnderMyControl;
            if (DebugMode) isSpawnedPlayerUnderMyControl = Int32.Parse(playerInstance.Name) > 0;
            else
            {
                if (NetworkHandler.IsServer()) return;
                isSpawnedPlayerUnderMyControl = Multiplayer.MultiplayerPeer.GetUniqueId() == playerInstance.GetMultiplayerAuthority();
            }

            if (ControlledPlayer == null && isSpawnedPlayerUnderMyControl)
            {
                ControlledPlayer = playerInstance;
                playerInstance.SetupCamera(true);
            }
        }

        private void setScoringBall(Ball ball) { _scoringBall = ball; }

        private bool isServerHost() { return NetworkHandler.IsHost() || NetworkHandler.IsServer(); }

        private void OnSpawnBtnPressed()
        {
            if ((isServerHost()) && _scoringBall == null) _shootPoint.SpawnBall( new Vector3(0, 0, 1));
        }

		private void onRandomBuffBtnPressed()
		{
			if (_spawnableBuffs.Count > 0)
				spawnBuff(_spawnableBuffs.PickRandom(), Vector3.Zero);
		}

		private void spawnBuff(BallModifier modifier, Vector3 spawnCoords)
		{
			BuffScene buffScene = modifier.ModifierScene.Instantiate() as BuffScene;
			buffScene.Position = spawnCoords;
			buffScene.Buff = modifier; // HACK
			// _arena.AddChild(buffScene);
			GetParent().AddChild(buffScene);
		}

    }
}
