using Silk.NET.Maths;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Text;
using TheAdventure.Entities.Objects;

namespace TheAdventure.Entities.Wall
{
    internal class Wall : GameObject
    {
        public Wall(int x, int y, int w,int h) : base(x,y,w,h)
        {}

        public override unsafe void Render (Sdl sdl, Renderer* render, TheAdventure.Camera.Camera camera)
        {
            sdl.SetRenderDrawColor(render, 100, 100, 100, 255);
            var model = new Rectangle<int>((int)X, (int)Y, (int)W, (int)H);
            var screenRect = camera.ToScreenCoordinates(model);
            model = new Rectangle<int>((int)screenRect.Origin.X, (int)screenRect.Origin.Y, 
                (int)screenRect.Size.X, (int)screenRect.Size.Y);
            sdl.RenderFillRect(render, ref model);
        }

        public override void Update(float delta)
        {
            return;
        }

        public override bool interacts(Entity another)
        {
            if (another is Wall)
            {
                return false;
            }
            if (this.collision(another))
            {
                if (another is Gun.Projectile.Projectile bullet)
                {
                    bullet.collided();
                    return true;
                }
                else
                {
                    pushBack(another);
                }
                return true;
            }
            return false;
        }

        private void pushBack(Entity another)
        {
            float wCenterX = X + (W / 2.0f);
            float wCenterY = Y+ (H / 2.0f);
            float eCenterX = another.X + (another.W / 2.0f);
            float eCenterY = another.Y + (another.H / 2.0f);

            float dx = eCenterX - wCenterX;
            float dy = eCenterY - wCenterY;

            float minDistanceX = (W / 2.0f) + (another.W / 2.0f);
            float minDistanceY = (H/2.0f) + (another.H/2.0f);

            float overlapX = minDistanceX - Math.Abs(dx);
            float overlapY = minDistanceY - Math.Abs(dy);

            if (overlapX<overlapY)
            {
                if (dx>0)
                {
                    another.X = this.X+ this.W;
                }
                else
                {
                    another.X = this.X- another.W;
                }
            }
            else
            {
                if (dy>0)
                {
                    another.Y = this.Y + this.H;
                }
                else
                {
                    another.Y = this.Y - another.H;
                }
            }
        }
    }
}
