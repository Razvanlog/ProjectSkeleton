using Silk.NET.Maths;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TheAdventure.Entities.Enemies
{
    internal class BasicEnemy : Entity
    {
        private float speed = 10f;
        private int damage = 15;
        private Player.Player player;
        private int points = 100;
        private int textureId;
        private GameRenderer.TextureInfo textureInfo;
        public int getScore()
        {
            return points;
        }
        public BasicEnemy(float x, float y, float size, float speed, Player.Player player) : base()
        {
            this.X = x;
            this.Y = y;
            this.W = size;
            this.H = size;
            this.speed = speed;
            this.player = player;
            this.textureId = GameRenderer.GameRenderer.LoadTexture(System.IO.Path.Combine("Assets", "enemy.png"), out this.textureInfo);
        }

        override public void Update(float delta)
        {
            float dx = player.X-this.X;
            float dy = player.Y-this.Y;

            if (!isDead())
            {
                damageFlashInd -= delta;
            }
            double angle = Math.Atan2(dy, dx);

            this.X += (float)(Math.Cos(angle) * speed * delta);
            this.Y += (float)(Math.Sin(angle) * speed * delta);
        }

        public override unsafe void Render(Sdl sdl, Renderer* renderer, TheAdventure.Camera.Camera camera)
        {
            var worldRect = new Rectangle<int>((int)X, (int)Y, (int)W, (int)H);
            var screenRect = camera.ToScreenCoordinates(worldRect);
            var srcRect = new Rectangle<int>(0, 0, textureInfo.Width, textureInfo.Height);
            var destrect = new Rectangle<int>((int)screenRect.Origin.X, (int)screenRect.Origin.Y, (int)screenRect.Size.X, (int)screenRect.Size.Y);
            if (!isDead())
            {
                byte r=255,g=255, b=255;
                if (damageFlashInd>0)
                {
                    g = 0;
                    b=0;
                }
                GameRenderer.GameRenderer.DrawTexture(textureId, srcRect, destrect, 0,r,g,b);
            }
        }

        public override bool interacts(Entity another)
        {
            if (another is Player.Player player && this.collision(player))
            {
                player.takeDamage(damage);
                return true;
            }
            else if (another is BasicEnemy otherEnemy && this.collision(otherEnemy))
            {
                float centerX1 = this.X + this.W / 2;
                float centerY1 = this.Y + this.H / 2;
                float centerX2= otherEnemy.X + otherEnemy.W / 2;
                float centerY2 = otherEnemy.Y + otherEnemy.H / 2;

                float dx = centerX1 - centerX2;
                float dy = centerY1 - centerY2;
                if (dx == 0 && dy == 0)
                {
                    dx = 0.1f;
                }

                double pushAngle = Math.Atan2(dy, dx);

                float force = 2.5f;

                this.X += (float)(Math.Cos(pushAngle) * force);
                this.Y += (float)(Math.Cos(pushAngle) * force);
                return true;
            }
            return false;
        }
    }
}
