
using Godot;
using System;
using System.Threading.Tasks;

namespace SpaceBallZ
{
    enum Team { Team1, Team2 }

    public partial class Manager : Node
    {

        static Manager ManagerInstance = new Manager();


        [Export]
        private Arena _arena;
        [Export]
        private ShootPoint _shootPoint;

        [Export]
        private Label Team1ScoreLabel;
        [Export]
        private Label Team2ScoreLabel;

        [Export]
        private PlayerSpawner _playerSpawner;


        protected Player ControlledPlayer;

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

        public  override void _Ready()
        {
            CheckIfAllValuesSet();
			// _playerSpawner.Connect(PlayerSpawner.SignalName.PlayerSpawned, Callable.From<Player>(SetPlayerCamera));
			_playerSpawner.PlayerSpawned += OnPlayerSpawned;

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
                GD.Print("Team 2 scored!");
                ChangeScores(Team.Team2, 2);
            }
			_scoringBall.QueueFree();
			_shootPoint.SpawnBall();
        }

		private void OnPlayerSpawned(Player playerInstance)
		{
			Task.Run(()=> SetPlayerCamera(playerInstance));
		}

		private async Task SetPlayerCamera(Player playerInstance)
		{
			await ToSignal(_playerSpawner, Player.SignalName.Ready);
			if (Multiplayer.IsServer()) return;
			bool isSpawnedPlayerUnderMyControl = Multiplayer.MultiplayerPeer.GetUniqueId() == playerInstance.GetMultiplayerAuthority();
			if (Variant.From(ControlledPlayer).VariantType == Variant.Type.Nil && isSpawnedPlayerUnderMyControl)
			{
				ControlledPlayer = playerInstance;
				playerInstance.SetupCamera(true);
			}
		}

    }
}
