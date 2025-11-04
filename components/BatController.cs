using Godot;

namespace SpaceBallZ
{
    public partial class BatController : Node
    {
        [Export] private Node3D _batMesh;
        [Export] private float _moveSpeed = 5f;
        [Export] private float _returnSpeed = 3f;
        [Export] private float _maxOffset = 1.5f;

        private Vector3 _defaultPosition;
        private bool _isDragging = false;
        private Vector2 _mouseStartPos;
        private Vector3 _targetOffset = Vector3.Zero;

        public override void _Ready()
        {
            if (_batMesh == null)
                _batMesh = GetParent<Node3D>();

            _defaultPosition = _batMesh.Position;
        }

        public override void _Process(double delta)
        {
            if (!IsMultiplayerAuthority()) return;
            GD.Print("BatController process running");
            if (Input.IsActionJustPressed("Mouse_LMB"))
            {
                _isDragging = true;
                _mouseStartPos = GetViewport().GetMousePosition();
            }

            if (Input.IsActionJustReleased("Mouse_LMB"))
            {
                _isDragging = false;
            }

            if (_isDragging)
            {
                Vector2 mouseDelta = GetViewport().GetMousePosition() - _mouseStartPos;

                Vector3 desiredOffset = new Vector3(mouseDelta.X, -mouseDelta.Y, 0) * 0.01f;
                desiredOffset = desiredOffset.LimitLength(_maxOffset);
                _targetOffset = desiredOffset;
            }
            else
            {

                _targetOffset = _targetOffset.Lerp(Vector3.Zero, (float)(_returnSpeed * delta));
            }


            Vector3 newPosition = _batMesh.Position.Lerp(_defaultPosition + _targetOffset, (float)(_moveSpeed * delta));
            _batMesh.Position = newPosition;
        }
    }
}
