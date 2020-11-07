using System.Linq;
using Game.System;
using Game.System.Map;
using Helper;
using UnityEngine;

namespace Game.Entity
{
    public static class ControlChecker
    {
        private static bool IsMoveControlAllowedOn(MapElement element)
        {
            return element.Tile != Tile.Wall;
        }

        public readonly struct ControlCheckResult
        {
            public readonly bool IsValid;
            public readonly EntityPosition ApplicablePosition;

            private ControlCheckResult(bool isValid, EntityPosition applicablePosition)
            {
                IsValid = isValid;
                ApplicablePosition = applicablePosition;
            }

            public static ControlCheckResult Invalid => new ControlCheckResult(false, default);
            public static ControlCheckResult ApplicableAt(EntityPosition position) => new ControlCheckResult(true, position);
        }

        public static ControlCheckResult CheckControlValid(MapField field, EntityPosition pos, EntityMove move, EntityControl entityControl)
        {
            // 要求した操作が「停止」の場合
            if (entityControl.Pattern == ControlPattern.Stop)
                return ControlCheckResult.ApplicableAt(pos);

            var controlAxis = entityControl.GetDirection().GetAxis();
            var constantAxis = controlAxis.CrossAxis();

            var checkPositionsControlAxis = RangeHelper.GetIntegerRangeWide(pos.GetAxisValue(controlAxis),
                    pos.GetAxisValue(controlAxis) + entityControl.GetDirection().GetVector().GetAxisValue(controlAxis))
                .Except(new[] {Mathf.FloorToInt(pos.GetAxisValue(controlAxis))})
                .ToArray();
            var checkPositionsConstantAxis = RangeHelper.GetIntegerRange(pos.GetAxisValue(constantAxis),
                pos.GetAxisValue(constantAxis) + move.GetAxisValue(constantAxis));

            foreach (var checkPositionConstantAxis in checkPositionsConstantAxis)
            {
                var hasControlAllow = checkPositionsControlAxis.Length <= 0 ||
                                      !checkPositionsControlAxis.All(checkPositionControlAxis =>
                                      {
                                          var tilePosition = GridPosition.FromVector(
                                              AxisHelper.GetVector(checkPositionControlAxis, checkPositionConstantAxis,
                                                  controlAxis));
                                          var mapElement = field.GetElement(tilePosition);
                                          return IsMoveControlAllowedOn(mapElement);
                                      });
                if (checkPositionConstantAxis.IsRange(0, field.Width - 1) && hasControlAllow)
                    continue;

                var controlApplicablePosition = new EntityPosition(controlAxis, pos.GetAxisValue(controlAxis),
                    checkPositionConstantAxis);
                return ControlCheckResult.ApplicableAt(controlApplicablePosition);
            }

            return ControlCheckResult.Invalid;
        }
    }
}