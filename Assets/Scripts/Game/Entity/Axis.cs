using System.Numerics;
using Game.System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Game.Entity
{
    public enum Axis
    {
        None,
        X,
        Y
    }

    public static class AxisHelper {
        public static float GetAxisValue(this Vector2 vec, Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return vec.x;
                case Axis.Y:
                    return vec.y;
            }

            return 0f;
        }

        public static int GetAxisValue(this Vector2Int vec, Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return vec.x;
                case Axis.Y:
                    return vec.y;
            }

            return 0;
        }

        public static Axis CrossAxis(this Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return Axis.Y;
                case Axis.Y:
                    return Axis.X;
            }

            return Axis.None;
        }

        public static Vector2 GetVector(float firstAxisValue, float secondAxisValue, Axis firstAxis)
        {
            switch (firstAxis)
            {
                case Axis.X:
                    return new Vector2(firstAxisValue, secondAxisValue);
                case Axis.Y:
                    return new Vector2(secondAxisValue, firstAxisValue);
            }

            return Vector2.zero;
        }

        public static Vector2Int GetVector(int firstAxisValue, int secondAxisValue, Axis firstAxis)
        {
            switch (firstAxis)
            {
                case Axis.X:
                    return new Vector2Int(firstAxisValue, secondAxisValue);
                case Axis.Y:
                    return new Vector2Int(secondAxisValue, firstAxisValue);
            }

            return Vector2Int.zero;
        }

        public static Axis GetAxis(this Vector2 vector)
        {
            if (vector == Vector2.zero) return Axis.None;
            return Mathf.Abs(vector.x) > Mathf.Abs(vector.y) ? Axis.X : Axis.Y;
        }

        public static Axis GetAxis(this Direction direction)
        {
            if (direction == Direction.Left || direction == Direction.Right) return Axis.X;
            if (direction == Direction.Up || direction == Direction.Down) return Axis.Y;
            return Axis.None;
        }
    }
}