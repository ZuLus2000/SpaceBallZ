using Godot;
using System;

namespace SpaceBallZ
{
    public partial class BallSpeedIncrease : BallModifier
    {
        protected override String _name { get { return "BallSpeedIncrease"; } }

        [Export]
        protected override int _duration { get; set; }

        [Export]
        private float _multiplySpeedValue;

        public override void MakeEffect(Node applicant)
        {
            Ball ball = applicant as Ball;
            ball.Speed = ball.InitialSpeed * _multiplySpeedValue;
			ball.BallColor = new Color("Green");
        }

        public override void Clean(IBuffable applicant)
        {
            Ball ball = applicant as Ball;
            ball.Speed = ball.InitialSpeed;
        }

    }
}

