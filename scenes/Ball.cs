
using Godot;
using static CustomMath.CustomMath;
using System.Collections.Generic;

namespace SpaceBallZ
{
    public partial class Ball : RigidBody3D, IBuffable
    {
        public float _initialSpeed = 0f;
        public float InitialSpeed
        {
            get { return _initialSpeed; }
            set
            {
                if (InitialSpeed != 0f)
                {
                    GD.PushWarning("Repeating assign of InitialSpeed!");
                    return;
                }
                _initialSpeed = value;
            }

        }

        public Color BallColor 
		{
			get { return ((FindChild("CSGSphere3D") as CsgSphere3D).MaterialOverride as StandardMaterial3D).AlbedoColor; }
			set { ((FindChild("CSGSphere3D") as CsgSphere3D).MaterialOverride as StandardMaterial3D).AlbedoColor = value; }
		}

        public float Speed { get; set; }
        public double StraightAngleMargin = 0.1;

        public HashSet<BallModifier> ActiveBuffs { get; private set; }

        public override void _Ready()
        {
            Speed = InitialSpeed;
        }

        void IBuffable.UpdateBuffs() { }


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
