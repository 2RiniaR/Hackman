using UniRx;
using UnityEngine;
using System.Linq;

namespace Hackman.Game.Player {
    public static class MoveChecker {

        private static bool IsMoveAllowedOn(Map.Tile tile) {
            return tile != Map.Tile.Wall;
        }

        /// <summary>
        /// 移動方向の変更が可能であるかを返す
        /// </summary>
        public static bool CheckMoveValid(
            Map.Tile[,] mapTiles,
            Vector2 pos,
            Vector2 move,
            out Vector2 fixedPosition
        ) {
            // プレイヤーの判定サイズ
            Vector2 size = new Vector2(1f, 1f);
            // マップの縦横の長さ
            Vector2Int mapSize = new Vector2Int(mapTiles.GetLength(0), mapTiles.GetLength(1));

            // xとy両方の指定がある場合、大きさが大きいほうのみ適用する
            if (Mathf.Abs(move.x) >= Mathf.Abs(move.y)) {
                int[] placingPositionsX = IntegerRangeHelper.GetIntegerRange2(pos.x, pos.x + size.x);
                float minX = Mathf.Min(pos.x, pos.x + move.x);
                float maxX = Mathf.Max(pos.x, pos.x + move.x) + size.x;
                int[] checkPositionsX = IntegerRangeHelper.GetIntegerRange2(minX, maxX)
                    .Except(placingPositionsX)
                    .ToArray();
                int[] checkPositionsY = IntegerRangeHelper.GetIntegerRange2(pos.y, pos.y + size.y);

                foreach (int x in checkPositionsX) {
                    if (
                        x.IsRange(0, mapSize.x - 1) &&
                        (checkPositionsY.Length > 0 && checkPositionsY.Any(y => y.IsRange(0, mapSize.y - 1) && !IsMoveAllowedOn(mapTiles[x, y])))
                    ) {
                        if (move.x < 0f) fixedPosition = new Vector2(x + 1f, pos.y);
                        else fixedPosition = new Vector2(x - size.x, pos.y);
                        return false;
                    }
                }

                fixedPosition = pos + move;
                return true;
            } else {
                int[] placingPositionsY = IntegerRangeHelper.GetIntegerRange2(pos.y, pos.y + size.y);
                float minY = Mathf.Min(pos.y, pos.y + move.y);
                float maxY = Mathf.Max(pos.y, pos.y + move.y) + size.y;
                int[] checkPositionsY = IntegerRangeHelper.GetIntegerRange2(minY, maxY)
                    .Except(placingPositionsY)
                    .ToArray();
                int[] checkPositionsX = IntegerRangeHelper.GetIntegerRange2(pos.x, pos.x + size.x);

                foreach (int y in checkPositionsY) {
                    if (
                        y.IsRange(0, mapSize.y - 1) &&
                        (checkPositionsX.Length > 0 && checkPositionsX.Any(x => x.IsRange(0, mapSize.x - 1) && !IsMoveAllowedOn(mapTiles[x, y])))
                    ) {
                        if (move.y < 0f) fixedPosition = new Vector2(pos.x, y + 1f);
                        else fixedPosition = new Vector2(pos.x, y - size.y);
                        return false;
                    }
                }

                fixedPosition = pos + move;
                return true;
            }
        }

    }
}
