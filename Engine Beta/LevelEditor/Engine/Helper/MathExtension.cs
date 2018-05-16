using System;
using Microsoft.Xna.Framework;

namespace LevelEditor.Engine.Helper
{
    internal static class MathExtension
    {

        public static Vector2 Mix(Vector2 x, Vector2 y, float factor)
        {
            return x + factor * (y - x);
        }

        public static Vector3 Mix(Vector3 x, Vector3 y, float factor)
        {
            return x + factor * (y - x);
        }

        public static float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public static float Max(Vector3 vector)
        {
            return Math.Max(Math.Max(vector.X, vector.Y), Math.Max(vector.Y, vector.Z));
        }

    }
}
