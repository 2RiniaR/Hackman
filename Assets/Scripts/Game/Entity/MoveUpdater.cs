using Game.System.Map;
using UnityEngine;

namespace Game.Entity
{
    public class MoveUpdater
    {

        private readonly MapSystem _map;
        private readonly Entity _entity;

        public MoveUpdater(Entity entity, MapSystem map)
        {
            _entity = entity;
            _map = map;
        }

        private EntityMove GetFlameMove()
        {
            return EntityMove.FromDirection(_entity.CurrentAction.Value.GetDirection(),
                _entity.moveSpeed * Time.deltaTime);
        }

        public void UpdatePosition()
        {
            // 現在のフレームでの移動前の座標
            var currentPosition = _entity.GetEntityPosition();
            // 現在のフレームでの移動ベクトル
            var moveVector = GetFlameMove();
            // 現在のフレームでの操作状態
            var control = _entity.CurrentControl.Value;

            if (control.IsNone)
                ApplyMove(currentPosition, moveVector);
            else
                ApplyMoveWithControl(currentPosition, moveVector, control);
        }

        /// <summary>
        ///     移動に対する操作を適用する
        /// </summary>
        private void ApplyMoveWithControl(EntityPosition position, EntityMove move, EntityControl entityControl)
        {
            var controlCheckResult = ControlChecker.CheckControlValid(_map.Field.Value, position, move, entityControl);

            // 操作方向に壁があるなど、移動方向の変更ができない場合は通常移動を行う
            if (!controlCheckResult.IsValid)
            {
                ApplyMove(position, move);
                return;
            }

            var toControlPosition = controlCheckResult.ApplicablePosition.GetVector() - position.GetVector();
            var moveCheckResult = MoveChecker.CheckMoveValid(_map.Field.Value, position, EntityMove.FromVector(toControlPosition));

            // 曲がり角までにブロックなどがなく、阻まれていない場合は移動方向を変更できる
            if (moveCheckResult.AllowedPosition.GetVector() == controlCheckResult.ApplicablePosition.GetVector())
            {
                _entity.CurrentControl.Value = new EntityControl(ControlPattern.None);
                _entity.CurrentAction.Value = Action.FromControl(new EntityControl(entityControl.Pattern));
            }

            _entity.SetEntityPosition(moveCheckResult.AllowedPosition);
        }

        /// <summary>
        ///     操作なしの移動を適用する
        /// </summary>
        private void ApplyMove(EntityPosition position, EntityMove move)
        {
            // positionからmoveの方向に、移動可能な場所まで移動する
            var moveCheckResult = MoveChecker.CheckMoveValid(_map.Field.Value, position, move);
            _entity.SetEntityPosition(moveCheckResult.AllowedPosition);
        }
    }
}