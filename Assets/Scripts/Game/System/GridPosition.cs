using System.Collections.Generic;
using Game.Entity;
using UniRx;
using UnityEngine;

namespace Game.System
{
    public readonly struct GridPosition
    {
        public bool Equals(GridPosition other)
        {
            return _position.Equals(other._position);
        }

        public override bool Equals(object obj)
        {
            return obj is GridPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _position.GetHashCode();
        }

        private readonly Vector2Int _position;
        public int X => _position.x;
        public int Y => _position.y;

        public static bool operator ==(GridPosition l, GridPosition r) => l._position == r._position;
        public static bool operator !=(GridPosition l, GridPosition r) => !(l == r);

        public static GridPosition operator +(GridPosition l, GridPosition r) =>
            FromVector(l.GetVector() + r.GetVector());

        public static GridPosition operator -(GridPosition l, GridPosition r) =>
            FromVector(l.GetVector() - r.GetVector());

        public static GridPosition operator +(GridPosition l, Vector2Int r) =>
            FromVector(l.GetVector() + r);

        public static GridPosition operator -(GridPosition l, Vector2Int r) =>
            FromVector(l.GetVector() - r);

        public static GridPosition operator +(Vector2Int l, GridPosition r) => r + l;
        public static GridPosition operator -(Vector2Int l, GridPosition r) => r - l;

        public Vector2Int GetVector()
        {
            return _position;
        }

        private GridPosition(Vector2Int position)
        {
            _position = position;
        }

        public static GridPosition FromVector(Vector2Int vec) => new GridPosition(vec);
    }
}