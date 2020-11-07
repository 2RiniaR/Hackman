using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game.System
{
    public readonly struct Direction
    {
        public bool Equals(Direction other)
        {
            return _pattern == other._pattern;
        }

        public override bool Equals(object obj)
        {
            return obj is Direction other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int) _pattern;
        }

        private static readonly ReactiveDictionary<Pattern, Vector2Int> VectorMap =
            new ReactiveDictionary<Pattern, Vector2Int>(
                new Dictionary<Pattern, Vector2Int>
                {
                    {Pattern.Up, Vector2Int.up},
                    {Pattern.Right, Vector2Int.right},
                    {Pattern.Down, Vector2Int.down},
                    {Pattern.Left, Vector2Int.left},
                }
            );

        private static readonly ReactiveDictionary<Direction, Direction> ClockwiseMap =
            new ReactiveDictionary<Direction, Direction>(
                new Dictionary<Direction, Direction>
                {
                    {Up, Right},
                    {Right, Down},
                    {Down, Left},
                    {Left, Up},
                }
            );

        private static readonly ReactiveDictionary<Direction, Direction> AntiClockwiseMap =
            new ReactiveDictionary<Direction, Direction>(
                new Dictionary<Direction, Direction>
                {
                    {Up, Right},
                    {Right, Down},
                    {Down, Right},
                    {Left, Up},
                }
            );

        private static readonly ReactiveDictionary<Direction, Direction> InverseMap =
            new ReactiveDictionary<Direction, Direction>(
                new Dictionary<Direction, Direction>
                {
                    {Up, Down},
                    {Right, Left},
                    {Down, Up},
                    {Left, Right},
                }
            );

        private readonly Pattern _pattern;

        public static Direction Left => new Direction(Pattern.Left);
        public static Direction Right => new Direction(Pattern.Right);
        public static Direction Up => new Direction(Pattern.Up);
        public static Direction Down => new Direction(Pattern.Down);
        public static Direction None => new Direction(Pattern.None);
        public static IEnumerable<Direction> All => new[] {Up, Right, Down, Left};

        public static bool operator ==(Direction l, Direction r) => l._pattern == r._pattern;
        public static bool operator !=(Direction l, Direction r) => !(l == r);
        public Direction Clockwise => ClockwiseMap.ContainsKey(this) ? ClockwiseMap[this] : None;
        public Direction AntiClockwise => AntiClockwiseMap.ContainsKey(this) ? AntiClockwiseMap[this] : None;
        public Direction Inverse => InverseMap.ContainsKey(this) ? InverseMap[this] : None;

        public Vector2Int GetVector()
        {
            return VectorMap.ContainsKey(_pattern) ? VectorMap[_pattern] : Vector2Int.zero;
        }

        private Direction(Pattern pattern)
        {
            _pattern = pattern;
        }

        public static Direction FromVector(Vector2 vec)
        {
            if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            {
                if (vec.x > 0) return Right;
                if (vec.x < 0) return Left;
            }
            else
            {
                if (vec.y > 0) return Up;
                if (vec.y < 0) return Down;
            }
            return None;
        }

        private enum Pattern
        {
            None,
            Up,
            Right,
            Down,
            Left,
        }
    }
}