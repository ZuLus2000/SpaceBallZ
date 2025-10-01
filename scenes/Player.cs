
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

        private bool _xInverted = false;
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
            Vector3 moveDirection = DesiredDirection;
            if (_xInverted) moveDirection *= -1;
            if (_useFloatingPhysics)
            {
                ApplyInputForce(moveDirection * _moveSpeed);
                return;
            }
            if (moveDirection != Vector3.Zero) MoveAndCollide(moveDirection * _moveSpeed);
        }

        private void ApplyInputForce(Vector3 force)
        {
            ApplyCentralForce(force);
        }
    }
}
