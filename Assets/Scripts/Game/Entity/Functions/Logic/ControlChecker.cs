using UniRx;
using UnityEngine;
using System.Linq;

namespace Hackman.Game.Entity {
    public static class ControlChecker {

        private static bool IsMoveControlAllowedOn(Map.Tile tile) {
            return tile != Map.Tile.Wall;
        }

        public static bool CheckControlValid(
            Map.Tile[,] mapTiles,
            Vector2 pos,
            Vector2 move,
            Vector2Int direction,
            out Vector2 validPosition
        ) {
            // マップの縦横の長さ
            Vector2Int mapSize = new Vector2Int(mapTiles.GetLength(0), mapTiles.GetLength(1));

            // xとy両方の指定がある場合、大きさが大きいほうのみ適用する
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
                int[] checkPositionsX = IntegerRangeHelper.GetIntegerRangeWide(pos.x, pos.x + direction.x)
                    .Except(new int[] { Mathf.FloorToInt(pos.x) })
                    .ToArray();
                int[] checkPositionsY = IntegerRangeHelper.GetIntegerRange(pos.y, pos.y + move.y);
                foreach (int y in checkPositionsY) {
                    if (
                        !y.IsRange(0, mapSize.y - 1) ||
                        (checkPositionsX.Length > 0 && checkPositionsX.All(x => !x.IsRange(0, mapSize.x - 1) || IsMoveControlAllowedOn(mapTiles[x, y])))
                    ) {
                        validPosition = new Vector2(pos.x, y);
                        return true;
                    }
                }
                validPosition = Vector2.zero;
                return false;
            } else {
                int[] checkPositionsY = IntegerRangeHelper.GetIntegerRangeWide(pos.y, pos.y + direction.y)
                    .Except(new int[] { Mathf.FloorToInt(pos.y) })
                    .ToArray();
                int[] checkPositionsX = IntegerRangeHelper.GetIntegerRange(pos.x, pos.x + move.x);
                foreach (int x in checkPositionsX) {
                    if (
                        !x.IsRange(0, mapSize.x - 1) ||
                        (checkPositionsY.Length > 0 && checkPositionsY.All(y => !y.IsRange(0, mapSize.y - 1) || IsMoveControlAllowedOn(mapTiles[x, y])))
                    ) {
                        validPosition = new Vector2(x, pos.y);
                        return true;
                    }
                }
                validPosition = Vector2.zero;
                return false;
            }
        }

    }
}
