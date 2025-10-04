using Godot;

namespace SpaceBallZ
{
    public partial class WallCollider : Area3D
    {
        [Export]
        private bool IsScoring = false;
        [Export]
        private int TeamId = 0;


    }
}
