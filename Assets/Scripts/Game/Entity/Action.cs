using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.System;
using UnityEngine;
using UniRx;

namespace Game.Entity
{
    public readonly struct Action
    {
        private static readonly ReadOnlyDictionary<ActionPattern, Direction> DirectionMap =
            new ReadOnlyDictionary<ActionPattern, Direction>(
                new Dictionary<ActionPattern, Direction>
                {
                    {ActionPattern.MoveUp, Direction.Up},
                    {ActionPattern.MoveRight, Direction.Right},
                    {ActionPattern.MoveDown, Direction.Down},
                    {ActionPattern.MoveLeft, Direction.Left},
                    {ActionPattern.Stop, Direction.None}
                }
            );

        private static readonly ReadOnlyDictionary<ControlPattern, ActionPattern> ControlMap =
            new ReadOnlyDictionary<ControlPattern, ActionPattern>(
                new Dictionary<ControlPattern, ActionPattern>
                {
                    {ControlPattern.DirectionUp, ActionPattern.MoveUp},
                    {ControlPattern.DirectionRight, ActionPattern.MoveRight},
                    {ControlPattern.DirectionDown, ActionPattern.MoveDown},
                    {ControlPattern.DirectionLeft, ActionPattern.MoveLeft},
                    {ControlPattern.Stop, ActionPattern.Stop}
                }
            );

        public readonly ActionPattern Pattern;

        public Direction GetDirection()
        {
            return DirectionMap.ContainsKey(Pattern) ? DirectionMap[Pattern] : Direction.None;
        }

        public Action(ActionPattern pattern)
        {
            Pattern = pattern;
        }

        public static Action FromControl(EntityControl entityControl)
        {
            var actionPattern = ControlMap.ContainsKey(entityControl.Pattern) ? ControlMap[entityControl.Pattern] : default;
            return new Action(actionPattern);
        }
    }

    public enum ActionPattern
    {
        None,
        MoveUp,
        MoveRight,
        MoveDown,
        MoveLeft,
        Stop
    }
}