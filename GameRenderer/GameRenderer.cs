using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Silk.NET.SDL;
using TheAdventure.GameLogic;
using TheAdventure.Entities.Player;
using Silk.NET.Maths;
namespace TheAdventure.GameRenderer
{
    public readonly struct TextureInfo
    {
        public int Width { get; init; }
        public int Height { get; init; }
        public int PixelDataSize => Width * Height * 4;
    }

    internal class GameRenderer
    {
        private readonly Dictionary<int, IntPtr> texturePointers = new();
        private readonly Dictionary<int, TextureInfo> textureInfo = new();
        private int index = 0;
        private Sdl sdl;
        private IntPtr renderer;
        private GameLogic.GameLogic gameLogic;
        private ulong lastUpdate;
        private ulong performanceFreq;

        private static GameRenderer? instance;

        public GameRenderer(Sdl sdl, GameWindow gameWindow, GameLogic.GameLogic gameLogic)
        {
            this.sdl = sdl;
            this.renderer = gameWindow.CreateRenderer();
            this.gameLogic = gameLogic;
            this.lastUpdate = sdl.GetPerformanceCounter();
            this.performanceFreq = sdl.GetPerformanceFrequency();
            instance = this;
        }

        public void Render()
        {
            if (EntityManager.EntityManager.Instance==null || gameLogic.Camera==null)
            {
                return;
            }
            ulong currentTime = sdl.GetPerformanceCounter();
            float delta = (float)(currentTime - lastUpdate)/performanceFreq;
            lastUpdate = currentTime;

            unsafe
            {
                var r = (Renderer*)renderer;
                sdl.SetRenderDrawColor(r, 255, 255, 255, 255);
                sdl.RenderClear(r);
                gameLogic.RenderGround(this);
                gameLogic.RenderAll(delta, this);
                EntityManager.EntityManager.Instance.render(sdl, r, gameLogic.Camera);
                sdl.RenderPresent(r);
            }
        }

        public static void DrawTexture(int textureId, Rectangle<int> src, Rectangle<int> dst, double angleRadians=0, byte r=255, byte g=255, byte b=255)
        {
            if (instance!=null && instance.texturePointers.TryGetValue(textureId, out var textPtr))
            {
                unsafe
                {
                    instance.sdl.SetTextureColorMod((Texture*)textPtr, r, g, b);
                    double angleDegrees = angleRadians * (180.0 / Math.PI);
                    instance.sdl.RenderCopyEx(
                        (Renderer*)instance.renderer,
                        (Texture*)textPtr,
                        ref src,
                        ref dst,
                        angleDegrees,
                        null,
                        RendererFlip.None
                        );
                }
            }
        }

        public void RenderTexture(int textureId, Rectangle<int> src, Rectangle<int> dst)
        {
            if (texturePointers.TryGetValue(textureId, out var texturePointer))
            {
                unsafe
                {
                    sdl.RenderCopy((Renderer*)renderer, (Texture*)texturePointer, ref src, ref dst);
                }
            }
        }

        public static int LoadTexture (string fileName, out TextureInfo textureInfo)
        {
            if (instance == null)
            {
                throw new InvalidOperationException("game renderer must be init");
            }
            using var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            var image = Image.Load<Rgba32>(fStream);

            textureInfo = new TextureInfo() { Width = image.Width, Height = image.Height };
            var imageRawData = new byte[textureInfo.PixelDataSize];
            image.CopyPixelDataTo(imageRawData.AsSpan());

            unsafe
            {
                fixed (byte* data = imageRawData)
                {
                    var imageSurface = instance.sdl.CreateRGBSurfaceWithFormatFrom(data, textureInfo.Width, textureInfo.Height, 32, textureInfo.Width * 4, (uint)PixelFormatEnum.Rgba32);
                    var imageTexture = instance.sdl.CreateTextureFromSurface((Renderer*)instance.renderer, imageSurface);
                    instance.sdl.FreeSurface(imageSurface);


                    instance.texturePointers[instance.index] = (IntPtr)imageTexture;
                    instance.textureInfo[instance.index] = textureInfo;

                    return instance.index++;
                }
            }
        }
    }
}
