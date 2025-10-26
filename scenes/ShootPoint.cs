
using Godot;

namespace SpaceBallZ
{
    public partial class ShootPoint : Marker3D
    {

        [Export]
        private PackedScene _ballScene;

        [Export]
        private bool _isReversed = false;

        [Export(PropertyHint.Range, "-1,0,")]
        private float _minYDeviation = 0f;
        [Export(PropertyHint.Range, "0,1,")]
        private float _maxYDeviation = 0f;
        [Export(PropertyHint.Range, "-1,0,")]
        private float _minXDeviation = 0f;
        [Export(PropertyHint.Range, "0,1,")]
        private float _maxXDeviation = 0f;

        [Export]
        private Manager _manager;

        [Export]
        private float _initialSpeed = 0.1f;

        [Signal]
        public delegate void BallSpawnedEventHandler(Ball BallInstance);

        public override void _Ready()
        { }

        public void SpawnBall() { SpawnBall(GetRandVector()); }

        public void SpawnBall(Vector3 startVelociy)
        {
            Ball ballInstance = _ballScene.Instantiate() as Ball;
            ballInstance.Position = Position;
            ballInstance.LinearVelocity = startVelociy.Normalized();
            ballInstance.InitialSpeed = _initialSpeed;
            GetParent().CallDeferred(Node.MethodName.AddChild, ballInstance);
            EmitSignal(SignalName.BallSpawned, ballInstance);
        }

        private Vector3 GetRandVector()
        {
            int zReverseMult = 1;
            if (_isReversed) zReverseMult *= -1;

            float tempY = _maxYDeviation;
            _maxYDeviation = Mathf.Max(_minYDeviation, _maxYDeviation);
            _minYDeviation = Mathf.Min(_minYDeviation, tempY);

            var temp_x = _maxXDeviation;
            _maxXDeviation = Mathf.Max(_minXDeviation, _maxXDeviation);
            _minXDeviation = Mathf.Min(_minXDeviation, temp_x);

            RandomNumberGenerator rng = new RandomNumberGenerator();
            float yAxisAngle = (_maxYDeviation - _minYDeviation) * rng.Randf() + _minYDeviation;
            float xAxisAngle = (_maxXDeviation - _minXDeviation) * rng.Randf() + _minXDeviation;

            return new Vector3(xAxisAngle, yAxisAngle, zReverseMult).Normalized();
        }

    }


}
