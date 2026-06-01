using Silk.NET.Maths;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Text;
using TheAdventure.Entities.Player;

namespace TheAdventure.Entities.Objects.Goal
{
    internal class Goal : GameObject
    {
        public Goal(int x, int y,int w,int h) : base(x,y,w,h){ }
        
        public void onCollide(Player.Player player)
        {
            interacts(player);
        }
        
        override public unsafe void Render (Sdl sdl, Renderer* renderer, TheAdventure.Camera.Camera camera)
        {
            sdl.SetRenderDrawColor(renderer, 200, 200, 0, 0);
            var rect = new Rectangle<int>( (int)X, (int)Y, (int)W, (int)H );
            sdl.RenderFillRect(renderer, ref rect);
        }

        public override bool interacts(Entity another)
        {
            if (another is Player.Player player)
            {
                if (Collision(player))
                {
                    Program.winner = true;
                }
            }
            return false;
        }
        public override void Update(float delta)
        {
            return;
        }
    }
}
