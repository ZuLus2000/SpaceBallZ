using Godot;
// using System;
using Godot.Collections;

namespace SpaceBallZ
{
    public partial class AbstractArena : Node3D
    {
        [Export]
        private Array<WallCollider> ScoringSurfaceTeam1 = new Array<WallCollider>();
        [Export]
        private Array<WallCollider> ScoringSurfaceTeam2 = new Array<WallCollider>();

        [Signal]
        public delegate void TeamScoreEventHandler(int TeamId);


        private void teamScore(Variant teamId) { EmitSignal(SignalName.TeamScore, teamId); }

        public override void _Ready()
        {
            foreach (WallCollider wall in ScoringSurfaceTeam1)
            {
                // wall.Connect(WallCollider.SignalName.BodyEntered, Callable.From(() => teamScore(1)));
				wall.BodyEntered += (_body) => teamScore(1);
            }
            foreach (WallCollider wall in ScoringSurfaceTeam2)
            {
                // wall.Connect(WallCollider.SignalName.BodyEntered, Callable.From(() => teamScore(2)));
				wall.BodyEntered += (_body) => teamScore(2);
            }

			// WallCollider wall = ScoringSurfaceTeam2[0];
			// 	wall.BodyEntered += (_body) => teamScore(2);
		}


    }
}
