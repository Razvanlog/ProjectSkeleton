using Silk.NET.Maths;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
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
        private Rectangle<int> shootFrame;
        float currentShootPoseTimer = 0f;
        float poseTimer = 0.2f;

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
            this.textureId = GameRenderer.GameRenderer.LoadTexture(System.IO.Path.Combine("Assets", "doomguy.png"), out this.textureInfo);

            walkFrames = new Rectangle<int>[]
                {
                    new Rectangle<int>(18, 10, 87,116),
                    new Rectangle<int>(154, 10, 85, 116),
                    new Rectangle<int>(282, 10, 78, 116),
                    new Rectangle<int>(418, 10, 75, 116)
                };
            deathFrames = new Rectangle<int>[]
            {
                new Rectangle<int>(5, 250, 40, 50),
                new Rectangle<int>(50, 250, 40, 50),
                new Rectangle<int>(95, 250, 50, 50),
                new Rectangle<int>(150,250,60, 50),
                new Rectangle<int>(215, 250, 60, 50),
            };
            shootFrame = new Rectangle<int>(913, 528, 111, 104);
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
            if (hp<=0 && !isDying)
            {
                isDying = true;
                currentFrame = 0;
                frameTimer = 0f;
            }
            if (currentShootPoseTimer>0)
            {
                currentShootPoseTimer -=delta;
            }
            var input = InputKey.Instance;
            frameTimer += delta;

            if (isDying)
            {
                if (deathFrames!=null && frameTimer >= timePerFrame && currentFrame < deathFrames.Length -1)
                {
                    currentFrame++;
                    frameTimer = 0f;
                }
                if (deathFrames != null && currentFrame == deathFrames.Length - 1)
                {
                    finishedDeathAnimation = true;
                }
                return;
            }

            if (input == null)
            {
                return;
            }

            isMoving = false;
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
                isMoving = true;
                }
                if (input.isHeldDown(KeyCode.A))
                {
                    X -= speed * delta;
                isMoving = true;
                }
                if (input.isHeldDown(KeyCode.D))
                {
                    X += speed * delta;
                isMoving = true;
                }
                if (input.isHeldDown(KeyCode.S))
                {
                    Y += speed * delta;
                isMoving = true;
                }
                if (isMoving)
            {
                if (frameTimer >= timePerFrame)
                {
                    currentFrame++;
                    if (walkFrames!=null && currentFrame >= walkFrames.Length)
                    {
                        currentFrame = 0;
                    }
                    frameTimer = 0f;
                }
            }
            else
            {
                currentFrame = 0;
            }

                current.Update(delta);

                if (input.mouseButtonLeft && TheAdventure.EntityManager.EntityManager.Instance!=null)
                {
                
                    var bullets = current.Shoot();
                    if (bullets.Count>0)
                {
                    currentShootPoseTimer = poseTimer;
                }
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

            Rectangle<int> srcRect;
            bool rectAssigned = false;

            if (isDying && deathFrames != null)
            {
                rectAssigned = true;
                srcRect = deathFrames[currentFrame];
            }
            else if (currentShootPoseTimer>0)
            {
                rectAssigned = true;
                srcRect = shootFrame;
            }
            else if (walkFrames != null)
            {

                rectAssigned = true;
                if (isMoving)
                {
                    srcRect = walkFrames[currentFrame];
                }
                else
                {
                    srcRect = walkFrames[0];
                }
            }
            else
                srcRect = screenRect;
            var destRect = new Rectangle<int>((int)screenRect.Origin.X, (int)screenRect.Origin.Y,(int)screenRect.Size.X, (int)screenRect.Size.Y);
            if (!winner && rectAssigned)
            {
                byte r = 255, g = 255, b = 255;
                if (damageFlashInd>0)
                {
                    g = 0;
                    b = 0;
                }
                var flip = RendererFlip.None;
                if (Math.Abs(angle*(180.0/Math.PI))>90)
                {
                    flip = RendererFlip.Vertical;
                }
                GameRenderer.GameRenderer.DrawTexture(textureId, srcRect, destRect, angle,r,g,b, flip);
            }
        }

        public override bool interacts(Entity another)
        {
            return false;
        }
    }
}
