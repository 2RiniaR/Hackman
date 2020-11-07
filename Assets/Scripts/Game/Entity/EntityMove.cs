using Game.System;
using UnityEngine;

namespace Game.Entity
{
    public readonly struct EntityMove
    {
        public readonly Axis MovableAxis;
        public readonly float MovableAxisValue;

        public EntityMove(Axis movableAxis, float movableAxisValue)
        {
            MovableAxis = movableAxis;
            MovableAxisValue = movableAxisValue;
        }

        public static EntityMove FromVector(Vector2 vec)
        {
            if (vec == Vector2.zero) return new EntityMove(Axis.None, 0f);
            return Mathf.Abs(vec.x) > Mathf.Abs(vec.y)
                ? new EntityMove(Axis.X, vec.x)
                : new EntityMove(Axis.Y, vec.y);
        }

        public static EntityMove FromDirection(Direction direction, float magnitude)
        {
            return FromVector((Vector2)direction.GetVector() * magnitude);
        }

        public Vector2 GetVector()
        {
            switch (MovableAxis)
            {
                case Axis.X:
                    return new Vector2(MovableAxisValue, 0);
                case Axis.Y:
                    return new Vector2(0, MovableAxisValue);
            }
            return Vector2.zero;
        }

        public float GetAxisValue(Axis axis)
        {
            return GetVector().GetAxisValue(axis);
        }
    }
}