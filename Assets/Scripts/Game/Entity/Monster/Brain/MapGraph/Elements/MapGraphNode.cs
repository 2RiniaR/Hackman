using Game.System;
using UnityEngine;

namespace Game.Entity.Monster.Brain.Map
{
    public readonly struct MapGraphNode
    {
        /// <summary>
        ///     座標に対応付けられているか
        /// </summary>
        public readonly bool IsMappingToPosition;

        public readonly GridPosition MappedPosition;

        public static MapGraphNode Mapped(GridPosition position) => new MapGraphNode(true, position);
        public static MapGraphNode Unmapped => new MapGraphNode(false, GridPosition.FromVector(Vector2Int.zero));

        private MapGraphNode(bool isMappingToPosition, GridPosition mappedPosition)
        {
            IsMappingToPosition = isMappingToPosition;
            MappedPosition = mappedPosition;
        }
    }
}