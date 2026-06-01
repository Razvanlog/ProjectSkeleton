using Silk.NET.Maths;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Text;
using TheAdventure.Entities.Player;

namespace TheAdventure.Entities.Objects
{
    internal abstract class GameObject : Entity
    {

        public bool isDestroyed { get; set; } = false;

        protected GameObject(int x, int y, int w, int h) : base()
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public bool Collision(Entity entity)
        {
            return entity.X < X + W && entity.X + entity.W > X && entity.Y < Y + H && entity.Y + entity.H > Y;
        }

        public virtual void OnCollide(Player.Player player) {}
    }
}
