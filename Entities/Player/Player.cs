using Silk.NET.Maths;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TheAdventure.Entities.Objects.Goal;
using TheAdventure.Input.InputKey;

namespace TheAdventure.Entities.Player
{
    internal class Player : Entity
    {
        
        private float iframes=0;
        private float speed = 600f;
        private bool winner = false;
        private Gun.Gun current;
        private Gun.Gun primary;
        private Gun.Gun secondary;
        private int textureId;
        private GameRenderer.TextureInfo textureInfo;
        public double angle { get; private set; }

        public Player(float x, float y, float size, float speed) : base()
        {
            this.X = x;
            this.Y = y;
            this.W = size;
            this.H = size;
            this.speed = speed;
            this.winner = false;
            this.primary = new Gun.Shotgun.Shotgun(this);
            this.secondary = new Gun.Gun(this);
            this.current = primary;
            this.textureId = GameRenderer.GameRenderer.LoadTexture(System.IO.Path.Combine("Assets", "player.png"), out this.textureInfo);
        }

        public void takeDamage(int amount)
        {
            if (iframes <= 0)
            {
                Console.WriteLine("ouch");
                hp -= amount;
                iframes = 2.0f;
                damageFlashInd = 0.4f;
            }
        }

        override public void Update(float delta)
        {
            if (!winner)
            {
                iframes-=delta;
                damageFlashInd -= delta;
            }
            var input = InputKey.Instance;
            if (input == null)
            {
                return;
            }
            if (input.isHeldDown(KeyCode.One))
            {
                this.current = this.primary;
            }
            if (input.isHeldDown(KeyCode.Two))
            {
                this.current = this.secondary;
            }
            if (input.isHeldDown(KeyCode.W))
                {
                    Y -= speed * delta;
                }
                if (input.isHeldDown(KeyCode.A))
                {
                    X -= speed * delta;
                }
                if (input.isHeldDown(KeyCode.D))
                {
                    X += speed * delta;
                }
                if (input.isHeldDown(KeyCode.S))
                {
                    Y += speed * delta;
                }
                current.Update(delta);

                if (input.mouseButtonLeft && TheAdventure.EntityManager.EntityManager.Instance!=null)
                {
                    var bullets = current.Shoot();
                    foreach (var bullet in bullets)
                    {
                        TheAdventure.EntityManager.EntityManager.Instance.add(bullet);
                    }
                }

            float centerX = X + (W / 2);
            float centerY = Y + (H / 2);

            var mouseX = input.MouseX;
            var mouseY = input.MouseY;

            double deltaX = mouseX - centerX;
            double deltaY = mouseY - centerY;

            angle = Math.Atan2(deltaY, deltaX);
        }

        override public unsafe void Render (Sdl sdl, Renderer* render, TheAdventure.Camera.Camera camera)
        {
            var worldRect = new Rectangle<int>((int)X, (int)Y, (int)W, (int)H);
            var screenRect = camera.ToScreenCoordinates(worldRect);
            var srcRect = new Rectangle<int>(0, 0, textureInfo.Width, textureInfo.Height);
            var destrect = new Rectangle<int>((int)screenRect.Origin.X, (int)screenRect.Origin.Y, (int)screenRect.Size.X, (int)screenRect.Size.Y);
            if (!winner)
            {
                byte r = 255, g = 255, b = 255;
                if (damageFlashInd>0)
                {
                    g = 0;
                    b = 0;
                }
                GameRenderer.GameRenderer.DrawTexture(textureId, srcRect, destrect, angle,r,g,b);
            }
        }

        public override bool interacts(Entity another)
        {
            if (another is Goal goaly)
            {
                return goaly.interacts(this);
            }
            return false;
        }
    }
}
