using Godot;
using System.Collections.Generic;

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
			// SpawnFunction = GD.Convert(SpawnFunctionOverride, Variant.Type.Callable);
			SpawnFunction = new Callable(this, MethodName.SpawnFunctionOverride);
        }


		private struct SpawnData 
		{
			public int id;
			public Vector3 defaultCoordinates;
			public bool invertDirection;

			public SpawnData( int _id, Vector3 _defaultCoordinates, bool _invertDirection)
			{
				id = _id;
				defaultCoordinates = _defaultCoordinates;
				invertDirection = _invertDirection;
			}
		}

		private Node SpawnFunctionOverride(Variant spawnData)
		{
			Player player = SceneToSpawn.Instantiate() as Player;
			player.Name = spawnData.id.ToString();
			player.DefaultCoordinates = spawnData.defaultCoordinates;
			player.XInverted = spawnData.invertDirection;
			if (spawnData.invertDirection) player.RotateY(Mathf.Pi);
			return player;
		}

		public void SpawnPlayer(int id)
		{
			if (Multiplayer.IsServer()) return;
			GD.Print("Connected: " + id.ToString());
			GD.Print("Peers: " + Multiplayer.GetPeers().ToString());

			int peerCount = Multiplayer.GetPeers().Length;

			Marker3D spawnPoint;
			bool invertDirection = peerCount == 1;
			if (peerCount == 1) spawnPoint = Player1SpawnPoint;
			else if (peerCount == 2) spawnPoint = Player2SpawnPoint;
			else return;

			SpawnData sd = new SpawnData(id, spawnPoint.Position, invertDirection);
			
			Spawn(Variant.From(sd));



		}

    }
}
