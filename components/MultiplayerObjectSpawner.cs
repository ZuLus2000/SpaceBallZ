using Godot;
using Godot.Collections;

namespace SpaceBallZ
{
    public partial class MultiplayerObjectSpawner : MultiplayerSpawner
    {
        [Export]
        private PackedScene SceneToSpawn;

        [Export]
        private Marker3D Player1SpawnPoint;
        [Export]
        private Marker3D Player2SpawnPoint;

        [Signal]
        public delegate void PlayerSpawnedEventHandler(Player PlayerInstance);

        public bool DebugMode = false;

        public override void _Ready()
        {
            SpawnFunction = new Callable(this, MethodName.SpawnFunctionOverride);
            NetworkHandler.Instance.PeerConnected += SpawnPlayer;
        }


        public Node SpawnFunctionOverride(Dictionary spawnData)
        {
            Player player = SceneToSpawn.Instantiate() as Player;
            player.Name = spawnData["id"].ToString();
            player.DefaultCoordinates = (Vector3)spawnData["defaultCoordinates"];
            player.XInverted = (bool)spawnData["invertDirection"];
            // player.SetupMultiplayer((int) spawnData["id"]);
            if (player.XInverted) player.RotateY(Mathf.Pi);
            EmitSignal(SignalName.PlayerSpawned, player);
            return player;
        }

        public void SpawnPlayer(long id)
        {
            int peerCount;
            if (!DebugMode)
            {
                // production environment
                if (!(NetworkHandler.IsServer() || NetworkHandler.IsHost())) return;

                GD.Print("Connected: " + id.ToString());
                GD.Print("Peers: " + string.Join(",", Multiplayer.GetPeers()));

                peerCount = Multiplayer.GetPeers().Length;
                // else - test environment
            }
            else peerCount = (int)id;


            Marker3D spawnPoint;
            int isHost = NetworkHandler.IsHost() ? 1 : 0; // HACK: If it works - it WORKS
            bool invertDirection = peerCount == 1 - isHost;
            if (peerCount == 1 - isHost) spawnPoint = Player1SpawnPoint;
            else if (peerCount == 2 - isHost) spawnPoint = Player2SpawnPoint;
            else return;

            Dictionary dic = new Dictionary();
            dic["id"] = id;
            dic["defaultCoordinates"] = spawnPoint.Position;
            dic["invertDirection"] = invertDirection;

            Spawn(dic);
        }
    }
}
