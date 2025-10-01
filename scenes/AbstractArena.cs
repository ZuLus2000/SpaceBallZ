using Godot;
// using System;
using Godot.Collections;

namespace SpaceBallZ
{
    public partial class Arena : Node3D
    {
        [Export]
        private Array<WallCollider> ScoringSurfaceTeam1 = new Array<WallCollider>();
        [Export]
        private Array<WallCollider> ScoringSurfaceTeam2 = new Array<WallCollider>();
    }
}
