using System.Linq;
using Hackman.Game.Map;
using UnityEngine;

namespace Hackman.Game.Entity
{
    public static class MoveChecker
    {
        private static bool IsMoveAllowedOn(MapElement element)
        {
            return element.Tile != Tile.Wall;
        }

        /// <summary>
        ///     移動方向の変更が可能であるかを返す
        /// </summary>
        public static bool CheckMoveValid(
            MapField field,
            Vector2 pos,
            Vector2 move,
            out Vector2 fixedPosition
        )
        {
            // プレイヤーの判定サイズ
            var size = new Vector2(1f, 1f);

            // xとy両方の指定がある場合、大きさが大きいほうのみ適用する
            if (Mathf.Abs(move.x) >= Mathf.Abs(move.y))
            {
                var placingPositionsX = IntegerRangeHelper.GetIntegerRange2(pos.x, pos.x + size.x);
                var minX = Mathf.Min(pos.x, pos.x + move.x);
                var maxX = Mathf.Max(pos.x, pos.x + move.x) + size.x;
                var checkPositionsX = IntegerRangeHelper.GetIntegerRange2(minX, maxX)
                    .Except(placingPositionsX)
                    .ToArray();
                var checkPositionsY = IntegerRangeHelper.GetIntegerRange2(pos.y, pos.y + size.y);

                foreach (var x in checkPositionsX)
                {
                    if (!x.IsRange(0, field.Width - 1) || checkPositionsY.Length <= 0 || checkPositionsY.All(y =>
                        IsMoveAllowedOn(field.GetElement(x, y)))) continue;
                    fixedPosition = move.x < 0f ? new Vector2(x + 1f, pos.y) : new Vector2(x - size.x, pos.y);
                    return false;
                }

                fixedPosition = pos + move;
                return true;
            }
            else
            {
                var placingPositionsY = IntegerRangeHelper.GetIntegerRange2(pos.y, pos.y + size.y);
                var minY = Mathf.Min(pos.y, pos.y + move.y);
                var maxY = Mathf.Max(pos.y, pos.y + move.y) + size.y;
                var checkPositionsY = IntegerRangeHelper.GetIntegerRange2(minY, maxY)
                    .Except(placingPositionsY)
                    .ToArray();
                var checkPositionsX = IntegerRangeHelper.GetIntegerRange2(pos.x, pos.x + size.x);

                foreach (var y in checkPositionsY)
                {
                    if (!y.IsRange(0, field.Height - 1) || checkPositionsX.Length <= 0 ||
                        checkPositionsX.All(x => IsMoveAllowedOn(field.GetElement(x, y)))) continue;
                    fixedPosition = move.y < 0f ? new Vector2(pos.x, y + 1f) : new Vector2(pos.x, y - size.y);
                    return false;
                }

                fixedPosition = pos + move;
                return true;
            }
        }
    }
}