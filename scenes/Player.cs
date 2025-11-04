
using Godot;

namespace SpaceBallZ
{
	public partial class Player : RigidBody3D
	{
		[Export]
		private float _moveSpeed = 1f;
		[Export]
		private float _acceleration = 1f;
		[Export]
		private bool _useFloatingPhysics = true;

		private Camera3D _playerCamera;

		public bool XInverted = false;
		public Vector3 DefaultCoordinates;
		public Vector3 DesiredDirection = Vector3.Zero;


		public override void _Ready()
		{
			_playerCamera = GetNode<Camera3D>("%PlayerCamera");
		}

		public override void _EnterTree()
		{
			SetMultiplayerAuthority(System.Int32.Parse(Name.ToString()));
			Position = DefaultCoordinates;

		}

		public override void _PhysicsProcess(double delta)
		{
			if (!IsMultiplayerAuthority()) return;

			// направление движения от ввода
			Vector3 moveDirection = DesiredDirection.Normalized();
			if (XInverted) moveDirection.X *= -1;

			// применяем силу
			ApplyCentralForce(moveDirection * _moveSpeed);

			// ограничиваем линейную скорость (иначе RigidBody улетит)
			var velocity = LinearVelocity;
			float maxSpeed = 10f;
			if (velocity.Length() > maxSpeed)
				LinearVelocity = velocity.Normalized() * maxSpeed;
		}

		private void ApplyInputForce(Vector3 force)
		{
			ApplyCentralForce(force);
		}

		public void SetupCamera(bool state)
		{
			_playerCamera.Current = state;
		}
	}
}
