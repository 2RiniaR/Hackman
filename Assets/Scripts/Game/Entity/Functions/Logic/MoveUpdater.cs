using UniRx;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hackman.Game.Entity {
    public class MoveUpdater : IDisposable {

        struct MoveApplicationResult {
            public Vector2 AfterPosition;
        }

        private static readonly Vector2 size = new Vector2(1f, 1f);
        private static readonly ReactiveDictionary<MoveControl, Vector2Int> controlMoveMap = new ReactiveDictionary<MoveControl, Vector2Int>(
            new Dictionary<MoveControl, Vector2Int>() {
                { MoveControl.DirectionUp,    new Vector2Int(0, 1)  },
                { MoveControl.DirectionRight, new Vector2Int(1, 0)  },
                { MoveControl.DirectionDown,  new Vector2Int(0, -1) },
                { MoveControl.DirectionLeft,  new Vector2Int(-1, 0) },
            }
        );

        private CompositeDisposable onDispose = new CompositeDisposable();

        private readonly PositionStatus positionStatus;
        private readonly MoveControlStatus moveControlStatus;
        private readonly MoveStatus moveStatus;
        private readonly MoveSpeedStore speedStore;
        private readonly Map.MapSystem map;

        private readonly Subject<MoveUpdateResult> onUpdated = new Subject<MoveUpdateResult>();
        public IObservable<MoveUpdateResult> OnUpdated => onUpdated;

        public MoveUpdater(MoveControlStatus moveControlStatus, PositionStatus positionStatus, MoveStatus moveStatus, MoveSpeedStore speedStore, Map.MapSystem map) {
            this.moveControlStatus = moveControlStatus;
            this.positionStatus = positionStatus;
            this.moveStatus = moveStatus;
            this.speedStore = speedStore;
            this.map = map;
            Observable.EveryUpdate().Subscribe(_ => Update()).AddTo(onDispose);
        }

        public void Dispose() {
            onDispose.Dispose();
        }

        private void Update() {
            // 現在のフレームでの移動前の座標
            Vector2 currentPosition = positionStatus.Position;
            // 現在のフレームでの移動ベクトル
            Vector2 moveVector = moveStatus.GetFlameMoveVector();
            // 現在のフレームでの操作状態
            MoveControl control = moveControlStatus.Control;

            MoveApplicationResult result;
            bool isMoveControlRequested = controlMoveMap.TryGetValue(control, out Vector2Int controlDirection);
            if (isMoveControlRequested) {
                result = ApplyMoveWithControl(currentPosition, moveVector, controlDirection);
            } else {
                result = ApplyMove(currentPosition, moveVector);
            }

            onUpdated.OnNext(
                MoveUpdateResult.Applied(currentPosition, moveVector, control, result.AfterPosition)
            );
        }

        /// <summary>
        /// 移動に対する操作を適用する
        /// </summary>
        private MoveApplicationResult ApplyMoveWithControl(Vector2 position, Vector2 move, Vector2Int controlDirection) {
            bool isControlValid = ControlChecker.CheckControlValid(
                map.Field,
                position,
                move,
                controlDirection,
                out Vector2 controlPosition  // 移動方向の変更を行う地点(曲がり角)の座標
            );

            // 操作方向に壁があるなど、移動方向の変更ができない場合は通常移動を行う
            if (!isControlValid) {
                return ApplyMove(position, move);
            }

            Vector2 toControlPosition = controlPosition - position;
            bool isMovingToControlPositionAllowed = MoveChecker.CheckMoveValid(
                map.Field,
                position,
                toControlPosition,
                out var fixedPosition
            );

            // 曲がり角までにブロックなどがあり、阻まれている場合は移動方向を変更できない
            if (isMovingToControlPositionAllowed) {
                moveControlStatus.SetControl(MoveControl.None);
                moveStatus.SetDirection(controlDirection);
                moveStatus.SetSpeed(speedStore.MoveSpeed);
            }
            positionStatus.SetPosition(fixedPosition);

            return new MoveApplicationResult {
                AfterPosition = fixedPosition
            };
        }

        /// <summary>
        /// 操作なしの移動を適用する
        /// </summary>
        private MoveApplicationResult ApplyMove(Vector2 position, Vector2 move) {
            // positionからmoveの方向に、移動可能な場所まで移動する
            MoveChecker.CheckMoveValid(map.Field, position, move, out var fixedPosition);
            positionStatus.SetPosition(fixedPosition);

            return new MoveApplicationResult {
                AfterPosition = fixedPosition
            };
        }

    }
}
