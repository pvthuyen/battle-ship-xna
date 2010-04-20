using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleShip.Core.Utils
{
    class Utility
    {
        private static Random rand = new Random();
        public static int GetRandomInt(int min, int max)
        {
            return (rand.Next(min, max));
        }
        public static float GetRandomFloat(float min, float max)
        {
            return (((float)rand.NextDouble() * (max - min)) + min);
        }
        public static Vector2 GetRandomVector2(Vector2 min, Vector2 max)
        {
            return (new Vector2(
            GetRandomFloat(min.X, max.X),
            GetRandomFloat(min.Y, max.Y)));
        }

        public static Rectangle GetTitleSafeArea(GraphicsDevice graphicsDevice, float percent)
        {
            Rectangle retval = new Rectangle(graphicsDevice.Viewport.X,
            graphicsDevice.Viewport.Y,
            graphicsDevice.Viewport.Width,
            graphicsDevice.Viewport.Height);
            return retval;

        }
    }
}
