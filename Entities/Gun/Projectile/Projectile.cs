using Silk.NET.SDL;
using System;
using Silk.NET.Maths;
using System.Collections.Generic;
using System.Text;
using TheAdventure.Entities.Enemies;

namespace TheAdventure.Entities.Gun.Projectile
{
    internal class Projectile: Entity
    {
        private float speed = 1200f;
        private double angle;
        public float damage { get; protected set; } = 20.0f;

        private int textureId;
        private GameRenderer.TextureInfo textureInfo;
        override public bool isDead()
        {
            return hp <= 0;
        }

        public Projectile(float x, float y, double angle) : base()
        {
            this.hp = 0.5f;
            this.X = x;
            this.Y = y;
            this.W = 5;
            this.H = 5;
            this.angle = angle;

            this.textureId = GameRenderer.GameRenderer.LoadTexture(System.IO.Path.Combine("Assets", "bullet.png"), out this.textureInfo);
        }

        override public void Update(float delta)
        {
            this.X += (float)(Math.Cos(angle) * speed * delta);
            this.Y += (float)(Math.Sin(angle) * speed * delta);
            hp -= delta;
        }

        override public unsafe void Render (Sdl sdl, Renderer* render, TheAdventure.Camera.Camera camera)
        {
            var worldRect = new Rectangle<int>((int)X, (int)Y, (int)W, (int)H);
            var screenRect = camera.ToScreenCoordinates(worldRect);
            var srcRect = new Rectangle<int>(0, 0, textureInfo.Width, textureInfo.Height);
            var destrect = new Rectangle<int>((int)screenRect.Origin.X, (int)screenRect.Origin.Y, (int)screenRect.Size.X, (int)screenRect.Size.Y);
            if (!isDead())
            {
                GameRenderer.GameRenderer.DrawTexture(textureId, srcRect, destrect, 0);
            }
        }


        public override bool interacts(Entity another)
        {
            if (another is BasicEnemy enemy && this.collision(enemy))
            {
                collided();
                enemy.takeDamage(damage);
                return true;
            }
            return false;
        }

        public void collided()
        {
            this.hp = 0;
        }

        public void setDamage(int damage)
        {
            this.damage = damage;
        }
    }
}
