using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.System;
using UnityEngine;

namespace Game.Entity
{
    public readonly struct EntityControl
    {
        private static readonly ReadOnlyDictionary<ControlPattern, Direction> ControlMoveMap =
            new ReadOnlyDictionary<ControlPattern, Direction>(
                new Dictionary<ControlPattern, Direction>
                {
                    {ControlPattern.DirectionUp, Direction.Up},
                    {ControlPattern.DirectionRight, Direction.Right},
                    {ControlPattern.DirectionDown, Direction.Down},
                    {ControlPattern.DirectionLeft, Direction.Left},
                    {ControlPattern.Stop, Direction.None}
                }
            );

        private static readonly ReadOnlyDictionary<Vector2, ControlPattern> VectorControlMap =
            new ReadOnlyDictionary<Vector2, ControlPattern>(
                new Dictionary<Vector2, ControlPattern>
                {
                    {Vector2.left, ControlPattern.DirectionLeft},
                    {Vector2.up, ControlPattern.DirectionUp},
                    {Vector2.right, ControlPattern.DirectionRight},
                    {Vector2.down, ControlPattern.DirectionDown},
                    {Vector2.zero, ControlPattern.Stop}
                });

        public readonly ControlPattern Pattern;
        public readonly bool IsNone;

        public static EntityControl None => new EntityControl(ControlPattern.None);

        public Direction GetDirection()
        {
            return ControlMoveMap.ContainsKey(Pattern) ? ControlMoveMap[Pattern] : Direction.None;
        }

        public EntityControl(ControlPattern pattern)
        {
            Pattern = pattern;
            IsNone = Pattern == ControlPattern.None;
        }

        public static EntityControl DirectionFromVector(Vector2 direction)
        {
            var pattern = VectorControlMap.ContainsKey(direction) ? VectorControlMap[direction] : default;
            return new EntityControl(pattern);
        }
    }

    public enum ControlPattern
    {
        None,
        DirectionUp,
        DirectionRight,
        DirectionDown,
        DirectionLeft,
        Stop
    }
}