using System.Linq;
using Game.System;
using Game.System.Map;
using Helper;
using UnityEngine;

namespace Game.Entity
{
    public static class MoveChecker
    {
        private static bool IsMoveAllowedOn(MapElement element)
        {
            return element.Tile != Tile.Wall;
        }

        public readonly struct MoveCheckResult
        {
            public readonly EntityPosition AllowedPosition;

            private MoveCheckResult(EntityPosition allowedPosition)
            {
                AllowedPosition = allowedPosition;
            }

            public static MoveCheckResult AllowedTo(EntityPosition position) => new MoveCheckResult(position);
        }

        /// <summary>
        ///     移動方向の変更が可能であるかを返す
        /// </summary>
        public static MoveCheckResult CheckMoveValid(MapField field, EntityPosition pos, EntityMove move)
        {
            // 移動方向の軸
            var moveAxis = move.MovableAxis;
            // 固定軸(移動方向でないほうの軸)
            var constAxis = move.MovableAxis.CrossAxis();
            // 現在位置の移動軸成分の値
            var posMoveAxisValue = pos.GetAxisValue(moveAxis);
            // 現在位置の固定軸成分の値
            var posConstAxisValue = pos.GetAxisValue(constAxis);

            // このフレームでエンティティが移動する間に、エンティティの判定が存在する範囲のうち、移動軸の最小/最大値
            var minMoveAxisValue = Mathf.Min(posMoveAxisValue, posMoveAxisValue + move.MovableAxisValue);
            var maxMoveAxisValue = Mathf.Max(posMoveAxisValue, posMoveAxisValue + move.MovableAxisValue) +
                       Entity.Size.GetAxisValue(moveAxis);

            // 現在、エンティティが既に乗っているすべてのタイルの移動軸座標
            var placedMoveAxisPositions = RangeHelper.GetIntegerRange2(posMoveAxisValue,
                posMoveAxisValue + Entity.Size.GetAxisValue(moveAxis));

            // 移動可能かの判別が必要なすべてのタイルの移動軸座標
            var checkPositionsMoveAxisValue = RangeHelper.GetIntegerRange2(minMoveAxisValue, maxMoveAxisValue)
                .Except(placedMoveAxisPositions)  // 既に現在乗っているタイルは除外する
                .Where(v => v.IsRange(0, field.Width - 1));  // マップの範囲外のタイルは除外する

            // このフレームでエンティティの判定が存在する範囲のうち、固定軸の最小/最大値
            var minConstAxisValue = posConstAxisValue;
            var maxConstAxisValue = posConstAxisValue + Entity.Size.GetAxisValue(constAxis);

            // 移動可能かの判別が必要なすべてのタイルの固定軸座標
            var checkPositionsConstantAxisValue = RangeHelper.GetIntegerRange2(minConstAxisValue, maxConstAxisValue);

            // 判定が必要なすべてのタイルに対して、移動可能判定を行う
            // 移動軸方向に対して走査
            foreach (var movableAxisValue in checkPositionsMoveAxisValue)
            {
                // 固定軸方向に対して、すべてのタイルが移動可能ならば終了
                if (checkPositionsConstantAxisValue.Length <= 0 ||
                    checkPositionsConstantAxisValue.All(constantAxisValue =>
                    {
                        var checkTilePosition =
                            GridPosition.FromVector(AxisHelper.GetVector(movableAxisValue, constantAxisValue,
                                move.MovableAxis));
                        var checkMapElement = field.GetElement(checkTilePosition);
                        return IsMoveAllowedOn(checkMapElement);
                    })) continue;

                // 移動を阻害するマップ要素があった場合、その要素に密着するような位置を返す
                if (move.MovableAxisValue < 0f)
                {
                    var collisionPosition = new EntityPosition(moveAxis, movableAxisValue + 1, pos.ConstantAxisValue);
                    return MoveCheckResult.AllowedTo(collisionPosition);
                }
                else
                {
                    var collisionPosition = new EntityPosition(moveAxis, movableAxisValue - 1, pos.ConstantAxisValue);
                    return MoveCheckResult.AllowedTo(collisionPosition);
                }
            }

            // 移動を阻害するマップ要素がない場合、moveだけ移動した後の位置を返す
            var afterMovePosition = EntityPosition.FromVector(pos.GetVector() + move.GetVector());
            return MoveCheckResult.AllowedTo(afterMovePosition);
        }
    }
}