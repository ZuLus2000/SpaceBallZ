
using Godot;
using System;
using System.Threading.Tasks;

namespace SpaceBallZ
{
    enum Team { Team1, Team2 }

    public partial class Manager : Node
    {

        public static Manager ManagerInstance{ get; private set;}
        [Export]
        private AbstractArena _arena;
        [Export]
        private ShootPoint _shootPoint;

        [Export]
        private Label Team1ScoreLabel;
        [Export]
        private Label Team2ScoreLabel;

        [Export]
        private PlayerSpawner _playerSpawner;

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
            _arena.TeamScore += Scored;
            _shootPoint.BallSpawned += setScoringBall;
        }

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
                GD.Print("Team 2 scored!"); ChangeScores(Team.Team2, 2);
            }
            _scoringBall.QueueFree();
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

        private void setScoringBall(Ball ball)
        {
            _scoringBall = ball;
        }
		
		private void OnSpawnBtnPressed()
		{
			bool isHost = NetworkHandler.IsHost();
			bool isServer = NetworkHandler.IsServer();
			if ((isHost || isServer) && _scoringBall == null) _shootPoint.SpawnBall();
		}

    }
}
