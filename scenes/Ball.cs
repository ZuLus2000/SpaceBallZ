
using Godot;
using static CustomMath.CustomMath;
using System.Collections.Generic;

namespace SpaceBallZ
{
    public partial class Ball : RigidBody3D, IBuffable
    {
        public float InitialSpeed { get; private set; }
        public float Speed { get; set; }
        public double StraightAngleMargin = 0.1;

        public HashSet<BallModifier> ActiveBuffs { get; private set; }


        public Ball New(float initialSpeed, double straightAngleMargin)
        {
            InitialSpeed = initialSpeed;
            Speed = InitialSpeed;
            StraightAngleMargin = straightAngleMargin;
            return this;
        }

        void IBuffable.UpdateBuffs()
        {
            foreach (BallModifier buff in ActiveBuffs)
            {
                continue;
            }
        }


        public override void _PhysicsProcess(double delta)
        {
            if (!(NetworkHandler.IsHost() || NetworkHandler.IsServer())) return;
            LinearVelocity = LinearVelocity.Normalized() * Speed;
            KinematicCollision3D collision = MoveAndCollide(LinearVelocity);
            if (collision != null)
            {
                Vector3 bounceVelocity = LinearVelocity.Bounce(collision.GetNormal()) * 1;
                double newZ = Unclamp(bounceVelocity.Z, -StraightAngleMargin, StraightAngleMargin);
                bounceVelocity.Z = (float)newZ;
                LinearVelocity = bounceVelocity;
            }
        }
    }
}
