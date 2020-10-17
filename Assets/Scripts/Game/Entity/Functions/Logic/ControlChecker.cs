using UniRx;
using UnityEngine;
using System.Linq;
using Hackman.Game.Map;

namespace Hackman.Game.Entity {
    public static class ControlChecker {

        private static bool IsMoveControlAllowedOn(MapElement? element) {
            return !element.HasValue || IsMoveControlAllowedOn(element.Value);
        }

        private static bool IsMoveControlAllowedOn(MapElement element) {
            return element.Tile != Tile.Wall;
        }

        public static bool CheckControlValid(
            MapField field,
            Vector2 pos,
            Vector2 move,
            Vector2Int direction,
            out Vector2 validPosition
        ) {
            // xとy両方の指定がある場合、大きさが大きいほうのみ適用する
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
                int[] checkPositionsX = IntegerRangeHelper.GetIntegerRangeWide(pos.x, pos.x + direction.x)
                    .Except(new int[] { Mathf.FloorToInt(pos.x) })
                    .ToArray();
                int[] checkPositionsY = IntegerRangeHelper.GetIntegerRange(pos.y, pos.y + move.y);
                foreach (int y in checkPositionsY) {
                    if (
                        !y.IsRange(0, field.Height - 1) ||
                        (checkPositionsX.Length > 0 && checkPositionsX.All(x => IsMoveControlAllowedOn(field.GetElement(x, y))))
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
                        !x.IsRange(0, field.Width - 1) ||
                        (checkPositionsY.Length > 0 && checkPositionsY.All(y => IsMoveControlAllowedOn(field.GetElement(x, y))))
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
