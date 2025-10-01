using Godot;
using System;

namespace SpaceBallZ
{
    public partial class PlayerSpawner : Node
    {
		[Export]
		private PackedScene SceneToSpawn;

		[Export]
		private Marker3D Player1SpawnPoint;
		private Marker3D Player2SpawnPoint;

		[Signal]
		public delegate void PlayerSpawnedEventHandler(Player PlayerInstance);

    }
}
