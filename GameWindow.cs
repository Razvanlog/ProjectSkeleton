using System;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheAdventure
{
    internal class GameWindow
    {
        private IntPtr window;
        private Sdl sdl;

        public GameWindow(Sdl sdl, int width, int height)
        {
            this.sdl = sdl;
            unsafe
            {
                this.window = (IntPtr)sdl.CreateWindow(
                    "Game", Sdl.WindowposCentered, Sdl.WindowposCentered, width, height,
                    (uint)WindowFlags.Resizable | (uint)WindowFlags.AllowHighdpi);

                if (this.window == IntPtr.Zero)
                {
                    throw new Exception("Failed to create window");
                }
            }
        }
        
        public IntPtr CreateRenderer()
        {
            unsafe
            {
                var renderer = (IntPtr)sdl.CreateRenderer((Window*)window, -1, (uint)RendererFlags.Accelerated);
                sdl.RenderSetVSync((Renderer *)renderer, 1);
                return renderer;
            }
        }
        
        public void Destroy()
        {
            unsafe
            {
                sdl.DestroyWindow((Window*)window);
            }
        }
    }
}
