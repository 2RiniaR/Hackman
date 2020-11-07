using UnityEngine;

namespace Game.Entity
{
    public readonly struct EntityPosition
    {
        public bool Equals(EntityPosition other)
        {
            return MovableAxis == other.MovableAxis && MovableAxisValue.Equals(other.MovableAxisValue) &&
                   ConstantAxisValue == other.ConstantAxisValue;
        }

        public override bool Equals(object obj)
        {
            return obj is EntityPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) MovableAxis;
                hashCode = (hashCode * 397) ^ MovableAxisValue.GetHashCode();
                hashCode = (hashCode * 397) ^ ConstantAxisValue;
                return hashCode;
            }
        }

        public readonly Axis MovableAxis;
        public readonly float MovableAxisValue;
        public readonly int ConstantAxisValue;

        public EntityPosition(Axis movableAxis, float movableAxisValue, int constantAxisValue)
        {
            MovableAxis = movableAxis;
            MovableAxisValue = movableAxisValue;
            ConstantAxisValue = constantAxisValue;
        }

        public static EntityPosition FromVector(Vector2 vec)
        {
            var intVector = new Vector2Int(Mathf.FloorToInt(vec.x + 0.5f), Mathf.FloorToInt(vec.y + 0.5f));
            var delta = vec - intVector;
            // X, Y座標がともに整数のとき、Axis.Noneとする
            if (delta.magnitude <= float.Epsilon) return new EntityPosition(Axis.None, intVector.x, intVector.y);
            return Mathf.Abs(delta.x) > Mathf.Abs(delta.y)
                ? new EntityPosition(Axis.X, vec.x, intVector.y)
                : new EntityPosition(Axis.Y, vec.y, intVector.x);
        }

        public Vector2 GetVector()
        {
            switch (MovableAxis)
            {
                case Axis.X:
                    return new Vector2(MovableAxisValue, ConstantAxisValue);
                case Axis.Y:
                    return new Vector2(ConstantAxisValue, MovableAxisValue);
            }
            return new Vector2(MovableAxisValue, ConstantAxisValue);
        }

        public float GetAxisValue(Axis axis)
        {
            return GetVector().GetAxisValue(axis);
        }

        public static bool operator ==(EntityPosition l, EntityPosition r) =>
            l.MovableAxis == r.MovableAxis && l.MovableAxisValue.Equals(r.MovableAxisValue) &&
            l.ConstantAxisValue == r.ConstantAxisValue;

        public static bool operator !=(EntityPosition l, EntityPosition r) => !(l == r);
    }
}