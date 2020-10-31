using System;
using System.Collections.Generic;
using Hackman.Game.Map;
using UniRx;
using UnityEngine;

namespace Hackman.Game.Entity
{
    public class MoveUpdater
    {
        private static readonly Vector2 Size = new Vector2(1f, 1f);

        private static readonly ReactiveDictionary<MoveControl, Vector2Int> ControlMoveMap =
            new ReactiveDictionary<MoveControl, Vector2Int>(
                new Dictionary<MoveControl, Vector2Int>
                {
                    {MoveControl.DirectionUp, Vector2Int.up},
                    {MoveControl.DirectionRight, Vector2Int.right},
                    {MoveControl.DirectionDown, Vector2Int.down},
                    {MoveControl.DirectionLeft, Vector2Int.left},
                    {MoveControl.Stop, Vector2Int.zero}
                }
            );

        private readonly MapSystem _map;
        private readonly MoveControlStatus _moveControlStatus;
        private readonly MoveStatus _moveStatus;

        private readonly Subject<MoveUpdateResult> _onUpdated = new Subject<MoveUpdateResult>();
        private readonly MoveSpeedStore _speedStore;

        private readonly Transform _transform;

        public MoveUpdater(MoveControlStatus moveControlStatus, Transform transform, MoveStatus moveStatus,
            MoveSpeedStore speedStore, MapSystem map)
        {
            _moveControlStatus = moveControlStatus;
            _transform = transform;
            _moveStatus = moveStatus;
            _speedStore = speedStore;
            _map = map;
        }

        public IObservable<MoveUpdateResult> OnUpdated => _onUpdated;

        public void UpdatePosition()
        {
            // 現在のフレームでの移動前の座標
            var currentPosition = (Vector2) _transform.localPosition - Size / 2f;
            // 現在のフレームでの移動ベクトル
            var moveVector = _moveStatus.GetFlameMoveVector();
            // 現在のフレームでの操作状態
            var control = _moveControlStatus.Control;

            var isMoveControlRequested = ControlMoveMap.TryGetValue(control, out var controlDirection);
            var result = isMoveControlRequested
                ? ApplyMoveWithControl(currentPosition, moveVector, controlDirection)
                : ApplyMove(currentPosition, moveVector);

            _onUpdated.OnNext(
                MoveUpdateResult.Applied(currentPosition, moveVector, control, result.AfterPosition)
            );
        }

        /// <summary>
        ///     移動に対する操作を適用する
        /// </summary>
        private MoveApplicationResult ApplyMoveWithControl(Vector2 position, Vector2 move, Vector2Int controlDirection)
        {
            var isControlValid = ControlChecker.CheckControlValid(
                _map.Field,
                position,
                move,
                controlDirection,
                out var controlPosition // 移動方向の変更を行う地点(曲がり角)の座標
            );

            // 操作方向に壁があるなど、移動方向の変更ができない場合は通常移動を行う
            if (!isControlValid) return ApplyMove(position, move);

            var toControlPosition = controlPosition - position;
            var isMovingToControlPositionAllowed = MoveChecker.CheckMoveValid(
                _map.Field,
                position,
                toControlPosition,
                out var fixedPosition
            );

            // 曲がり角までにブロックなどがあり、阻まれている場合は移動方向を変更できない
            if (isMovingToControlPositionAllowed)
            {
                _moveControlStatus.SetControl(MoveControl.None);
                _moveStatus.SetDirection(controlDirection);
                _moveStatus.SetSpeed(_speedStore.MoveSpeed);
            }

            _transform.localPosition = fixedPosition + Size / 2f;

            return new MoveApplicationResult
            {
                AfterPosition = fixedPosition
            };
        }

        /// <summary>
        ///     操作なしの移動を適用する
        /// </summary>
        private MoveApplicationResult ApplyMove(Vector2 position, Vector2 move)
        {
            // positionからmoveの方向に、移動可能な場所まで移動する
            MoveChecker.CheckMoveValid(_map.Field, position, move, out var fixedPosition);
            _transform.localPosition = fixedPosition + Size / 2f;

            return new MoveApplicationResult
            {
                AfterPosition = fixedPosition
            };
        }

        private struct MoveApplicationResult
        {
            public Vector2 AfterPosition;
        }
    }
}