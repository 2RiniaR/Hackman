using System.Linq;
using Hackman.Game.Map;
using UnityEngine;

namespace Hackman.Game.Entity
{
    public static class ControlChecker
    {
        private static bool IsMoveControlAllowedOn(MapElement element)
        {
            return element.Tile != Tile.Wall;
        }

        public static bool CheckControlValid(
            MapField field,
            Vector2 pos,
            Vector2 move,
            Vector2Int direction,
            out Vector2 validPosition
        )
        {
            // 要求した操作が「停止」の場合
            if (direction.magnitude == 0)
            {
                validPosition = pos;
                return true;
            }

            // xとy両方の指定がある場合、大きさが大きいほうのみ適用する
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                var checkPositionsX = IntegerRangeHelper.GetIntegerRangeWide(pos.x, pos.x + direction.x)
                    .Except(new[] {Mathf.FloorToInt(pos.x)})
                    .ToArray();
                var checkPositionsY = IntegerRangeHelper.GetIntegerRange(pos.y, pos.y + move.y);
                foreach (var y in checkPositionsY)
                {
                    if (y.IsRange(0, field.Height - 1) && (checkPositionsX.Length <= 0 ||
                                                           !checkPositionsX.All(x =>
                                                               IsMoveControlAllowedOn(field.GetElement(x, y)))))
                        continue;
                    validPosition = new Vector2(pos.x, y);
                    return true;
                }

                validPosition = Vector2.zero;
                return false;
            }
            else
            {
                var checkPositionsY = IntegerRangeHelper.GetIntegerRangeWide(pos.y, pos.y + direction.y)
                    .Except(new[] {Mathf.FloorToInt(pos.y)})
                    .ToArray();
                var checkPositionsX = IntegerRangeHelper.GetIntegerRange(pos.x, pos.x + move.x);
                foreach (var x in checkPositionsX)
                {
                    if (x.IsRange(0, field.Width - 1) && (checkPositionsY.Length <= 0 ||
                                                          !checkPositionsY.All(y =>
                                                              IsMoveControlAllowedOn(field.GetElement(x, y)))))
                        continue;
                    validPosition = new Vector2(x, pos.y);
                    return true;
                }

                validPosition = Vector2.zero;
                return false;
            }
        }
    }
}