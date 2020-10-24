using UnityEngine;
using Hackman.Game.Map;
using System;
using System.Linq;
using UniRx;

namespace Hackman.Game.Entity.Player {
    public class DotEater : IDisposable {

        private const float dotEatableDistance = 0.1f;
        private static readonly Tile[] targetTiles = new Tile[] { Tile.Dot, Tile.PowerCookie };
        private static readonly Tile afterTile = Tile.Floor;

        private readonly CompositeDisposable onDispose = new CompositeDisposable();

        private readonly MapSystem map;

        public DotEater(MapSystem map, MoveUpdater moveUpdater) {
            this.map = map;
            moveUpdater.OnUpdated.Subscribe(EatDot).AddTo(onDispose);
        }

        public void Dispose() {
            onDispose.Dispose();
        }

        private void EatDot(MoveUpdateResult move) {
            var positions = GetNearIntergerPositions(move.AfterPosition, move.MoveResultVector, dotEatableDistance);
            foreach (var pos in positions) {
                if (!IsTargetTile(pos)) {
                    continue;
                }
                map.Field.UpdateElement(pos, new MapElement(afterTile));
            }
        }

        private bool IsTargetTile(Vector2Int position) {
            return targetTiles.Contains(map.Field.GetElement(position).Tile);
        }

        private static Vector2Int[] GetNearIntergerPositions(Vector2 position, Vector2 move, float mergin) {
            if (Mathf.Abs(move.x) >= Mathf.Abs(move.y)) {
                int y = Mathf.FloorToInt(position.y + mergin);
                return IntegerRangeHelper.GetIntegerRange(position.x - mergin, position.x + move.x + mergin)
                    .Select(x => new Vector2Int(x, y))
                    .ToArray();
            } else {
                int x = Mathf.FloorToInt(position.x + mergin);
                return IntegerRangeHelper.GetIntegerRange(position.y - mergin, position.y + move.y + mergin)
                    .Select(y => new Vector2Int(x, y))
                    .ToArray();
            }
        }

    }
}
