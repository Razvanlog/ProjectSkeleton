using Silk.NET.Maths;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace TheAdventure.Camera
{
    internal class Camera
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Rectangle<int> ToScreenCoordinates(Rectangle<int> rect)
        {
            return rect.GetTranslated(new Vector2D<int>(Width / 2 - X, Height / 2 - Y));
        }

        public Vector2D<int> toWorldCoordinates(Vector2D<int> point)
        {
            return point - new Vector2D<int>(Width / 2- X, Height / 2 - Y); 
        }
    }
}
