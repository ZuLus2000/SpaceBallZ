
using Godot;

namespace SpaceBallZ
{
    partial class InputControls : Node
    {
        private Player _controlled_body;

        public override void _Ready() { _controlled_body = GetParent() as Player; }

        public override void _PhysicsProcess(double delta)
        {
            Vector2 movementInput = Input.GetVector("Left", "Right", "Down", "Up");
            _controlled_body.DesiredDirection = new Vector3(movementInput.X, movementInput.Y, 0f);
        }

    }
}
