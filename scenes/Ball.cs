
using Godot;
using static CustomMath.CustomMath;

namespace SpaceBallZ
{
    public partial class Ball : RigidBody3D
    {
        public float Speed = 0f;
        public double StraightAngleMargin = 0.1;

        public override void _PhysicsProcess(double delta)
        {
            LinearVelocity = LinearVelocity.Normalized() * Speed;
            KinematicCollision3D collision = MoveAndCollide(LinearVelocity);
            if (collision != null)
            {
                Vector3 bounceVelocity = LinearVelocity.Bounce(collision.GetNormal()) * 1;
                double newZ = Unclamp(bounceVelocity.Z, -StraightAngleMargin, StraightAngleMargin);
                bounceVelocity.Z = (float) newZ;
                LinearVelocity = bounceVelocity;
            }
        }
    }
}
