using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheAdventure.Entities
{
    internal abstract class Entity
    {
        public float hp { get; protected set; } = 100;
        public float X { get; set; } = 600;
        public float Y { get; set; } = 600;
        public float W { get; protected set; } = 60;
        public float H { get; protected set; } = 60;
        public float damageFlashInd { get; protected set; } = 0f;
        public abstract unsafe void Render(Sdl sdl, Renderer* renderer, TheAdventure.Camera.Camera camera);
        public bool collision(Entity another)
        {
            return (this.X < another.X + another.W && this.X + this.W > another.X && this.Y < another.Y + another.H && this.Y + this.H > another.Y);
        }
        public abstract bool interacts(Entity another);
        public void takeDamage(float amount)
        {
            hp -= amount;
            damageFlashInd = 0.4f;
        }
        public abstract void Update(float delta);

        public virtual bool isDead()
        {
            return hp <= 0;
        }
    }
}
