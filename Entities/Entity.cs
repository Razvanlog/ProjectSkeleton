using Silk.NET.Maths;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        protected float frameTimer = 0f;
        protected int currentFrame = 0;
        protected float timePerFrame = 0.15f;
        protected bool isMoving = false;
        protected bool isDying = false;
        protected bool finishedDeathAnimation = false;

        protected Rectangle<int>[]? walkFrames;
        protected Rectangle<int>[]? deathFrames;
        public abstract unsafe void Render(Sdl sdl, Renderer* renderer, TheAdventure.Camera.Camera camera);
        public bool collision(Entity another)
        {
            return (this.X < another.X + another.W && this.X + this.W > another.X && this.Y < another.Y + another.H && this.Y + this.H > another.Y);
        }
        public abstract bool interacts(Entity another);
        public void takeDamage(float amount)
        {
            hp -= amount;
            if (hp < 0)
            {
                isDying = true;
            }
            damageFlashInd = 0.4f;
        }
        public abstract void Update(float delta);


        public virtual bool getIsDying()
        {
            return isDying;
        }
        public virtual bool isDead()
        {
            return finishedDeathAnimation;
        }
    }
}
