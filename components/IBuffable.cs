using Godot;
using System.Collections.Generic;

namespace SpaceBallZ
{
    public interface IBuffable
    {
        public abstract HashSet<BallModifier> ActiveBuffs { get; }

        public Timer RecieveBuff(BallModifier modifier, int duration)
        {
            foreach (BallModifier buff in ActiveBuffs)
            {
                if (buff == modifier) { CleanBuff(buff); break; }
                // Как будто бы CleanBuff лишний раз будет проверять наличие бафа в сете, 
                // но сложность такой операции O(1),
                // так что прирост производительности будет незначительным
            }

            Node reciever = this as Node;
            Timer timer = new Timer();
            reciever.AddChild(timer);
            ActiveBuffs.Add(modifier);
            timer.Timeout += () => CleanBuff(modifier);

            timer.Start(duration);
            return timer;
        }

        public void CleanBuff(BallModifier modifier)
        {
            if (!ActiveBuffs.Contains(modifier)) return; // Можно заменить Contains на Remove, поскольку Remove возвращает false, если элемента нет
            ActiveBuffs.Remove(modifier); // Оставлю так для читабельности
            modifier.Clean(this);
            modifier.RemoveTimer();
            modifier.Dispose();
        }


        public void UpdateBuffs()
        {
            foreach (BallModifier buff in ActiveBuffs)
            {
                // buff.
            }
        }
    }
}
