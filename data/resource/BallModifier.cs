using Godot;
using System;

namespace SpaceBallZ
{
    public abstract partial class BallModifier : Resource
    {
        protected abstract String _name { get; }

        protected abstract int _duration { get; set; }

        protected Timer _countdownTimer;

        public void ApplyToBuffable(IBuffable recipient)
        {
            _countdownTimer = recipient.RecieveBuff(this, _duration);
        }

        public void RemoveTimer()
        {
            if (_countdownTimer == null) return;

            _countdownTimer.Stop();
            _countdownTimer.QueueFree();
            _countdownTimer = null;
        }

        public abstract void MakeEffect(Node applicant);

        public abstract void Clean(IBuffable ball);

        public override bool Equals(object obj)
        {
            BallModifier other = obj as BallModifier;
            return _name.Equals(other._name);
        }

        public override int GetHashCode() { return (int)_name.Hash(); }
    }
}
