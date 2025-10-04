using Godot;
using Godot.Collections;

namespace SpaceBallZ
{
    public partial class PlayerSpawner : MultiplayerSpawner
    {
		[Export]
		private PackedScene SceneToSpawn;

		[Export]
		private Marker3D Player1SpawnPoint;
		[Export]
		private Marker3D Player2SpawnPoint;

		[Signal]
		public delegate void PlayerSpawnedEventHandler(Player PlayerInstance);


        public override void _Ready()
        {
			SpawnFunction = new Callable(this, MethodName.SpawnFunctionOverride);
			Multiplayer.PeerConnected += SpawnPlayer;
        }


		private Node SpawnFunctionOverride(Dictionary spawnData)
		{
			Player player = SceneToSpawn.Instantiate() as Player;
			player.Name = spawnData["id"].ToString();
			player.DefaultCoordinates = (Vector3) spawnData["defaultCoordinates"];
			player.XInverted = (bool) spawnData["invertDirection"];
			// player.SetupMultiplayer((int) spawnData["id"]);
			if (player.XInverted) player.RotateY(Mathf.Pi);
			EmitSignal(SignalName.PlayerSpawned, player);
			return player;
		}

		public void SpawnPlayer(long id)
		{
			if (!Multiplayer.IsServer()) return;

			GD.Print("Connected: " + id.ToString());
			GD.Print("Peers: " + string.Join(",", Multiplayer.GetPeers()));

			int peerCount = Multiplayer.GetPeers().Length;

			Marker3D spawnPoint;
			bool invertDirection = peerCount == 1;
			if (peerCount == 1) spawnPoint = Player1SpawnPoint;
			else if (peerCount == 2) spawnPoint = Player2SpawnPoint;
			else return;

			Dictionary dic = new Dictionary();
			dic["id"] = id;
			dic["defaultCoordinates"] = spawnPoint.Position;
			dic["invertDirection"] = invertDirection;

			Spawn(dic);
		}
    }
}
