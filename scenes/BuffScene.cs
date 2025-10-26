using Godot;
using System;

namespace SpaceBallZ
{
    [Tool]
    public partial class BuffScene : Node3D
    {
        private float _radius = 0.01f;

        [Export(PropertyHint.Range, "0.01,30,or_greater")]
        public float Radius
        {
            get { return _radius; }
            set { setRadius(value); }
        }

        private Area3D _area3D;
        private CollisionShape3D _collisionShape;
        private MeshInstance3D _meshInstance;

        public BallModifier Buff;

        public override void _Ready()
        {
            if (!Engine.IsEditorHint() && Buff == null) GD.PrintErr("Buff not set in " + this.ToString());
            _area3D = FindChild("Area3D") as Area3D;
            _collisionShape = _area3D.FindChild("CollisionShape3D") as CollisionShape3D;
            _meshInstance = FindChild("MeshInstance3D") as MeshInstance3D;
            if (_area3D != null) _area3D.BodyEntered += onBodyEntered;
        }

		protected void setRadius(float newRadius)
		{
                _radius = newRadius;
                if (_collisionShape == null) return;
                SphereShape3D shape = _collisionShape.Shape as SphereShape3D;
                if (_meshInstance == null) return;
                SphereMesh mesh = _meshInstance.Mesh as SphereMesh;

                if (shape != null)
                    shape.Radius = newRadius;
                if (mesh != null)
                {
                    mesh.Radius = newRadius;
                    mesh.Height = 2 * newRadius;
                }
		}

        private void onBodyEntered(Node3D body)
        {
            IBuffable buffable = body as IBuffable;
            if (buffable == null) return;

            Buff.MakeEffect(body);
        }
    }
}
