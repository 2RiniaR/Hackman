using System.Collections.Generic;
using Game.System;

namespace Game.Entity.Monster.Brain.Map
{
    public readonly struct MapGraphEdge
    {
        /// <summary>
        ///     座標に対応付けられているか
        /// </summary>
        public readonly bool IsMappingToPosition;

        public readonly IEnumerable<DirectPosition> MappedPositions;

        public static MapGraphEdge Mapped(in IEnumerable<DirectPosition> positions) => new MapGraphEdge(true, positions);
        public static MapGraphEdge Unmapped => new MapGraphEdge(false, new DirectPosition[0]);

        private MapGraphEdge(bool isMappingToPosition, in IEnumerable<DirectPosition> mappedPositions)
        {
            IsMappingToPosition = isMappingToPosition;
            MappedPositions = mappedPositions;
        }
    }

    public readonly struct DirectPosition
    {
        public readonly GridPosition Position;
        public readonly Direction Direction;

        public DirectPosition(GridPosition position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}