using UniRx;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hackman.Game.Entity {
    public class MoveUpdater : IDisposable {

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
        private readonly Map.MapSystem map;

        public MoveUpdater(MoveControlStatus moveControlStatus, PositionStatus positionStatus, MoveStatus moveStatus, Map.MapSystem map) {
            this.moveControlStatus = moveControlStatus;
            this.positionStatus = positionStatus;
            this.moveStatus = moveStatus;
            this.map = map;
            Observable.EveryUpdate().Subscribe(_ => MoveUpdate()).AddTo(onDispose);
        }

        public void Dispose() {
            onDispose.Dispose();
        }

        private void MoveUpdate() {
            // 現在のフレームでの操作状態
            MoveControl control = moveControlStatus.Control;
            // 現在のフレームでの移動前の座標
            Vector2 currentPosition = positionStatus.Position;
            // 現在のフレームでの移動ベクトル
            Vector2 moveVector = moveStatus.Direction * moveStatus.Speed * Time.deltaTime;

            bool isMoveControlRequested = controlMoveMap.TryGetValue(control, out Vector2Int controlDirection);
            if (isMoveControlRequested) {
                ApplyMoveControl(currentPosition, moveVector, controlDirection);
            } else {
                ApplyMove(currentPosition, moveVector);
            }
        }

        /// <summary>
        /// 移動に対する操作を適用する
        /// </summary>
        private void ApplyMoveControl(Vector2 position, Vector2 move, Vector2Int controlDirection) {
            bool isControlValid = ControlChecker.CheckControlValid(
                map.GetMapTiles(),
                position,
                move,
                controlDirection,
                out Vector2 controlPosition  // 移動方向の変更を行う地点(曲がり角)の座標
            );

            // Debug.Log(string.Join("\n", new string[] {
            //     "[CheckControlValid]",
            //     "    pos: (" + position.x.ToString("F5") + ", " + position.y.ToString("F5") + ")",
            //     "    mov: (" + move.x.ToString("F5") + ", " + move.y.ToString("F5") + ")",
            //     "    ctl: (" + controlDirection.x.ToString() + ", " + controlDirection.y.ToString() + ")",
            //     "    fix: (" + controlPosition.x.ToString("F5") + ", " + controlPosition.y.ToString("F5") + ")",
            //     "    ret: " + isControlValid
            // }));

            // 操作方向に壁があるなど、移動方向の変更ができない場合は通常移動を行う
            if (!isControlValid) {
                ApplyMove(position, move);
                return;
            }

            Vector2 toControlPosition = controlPosition - position;
            bool isMovingToControlPositionAllowed = MoveChecker.CheckMoveValid(
                map.GetMapTiles(),
                position,
                toControlPosition,
                out var fixedPosition
            );

            // Debug.Log(string.Join("\n", new string[] {
            //     "[CheckMoveValid]",
            //     "    pos: (" + position.x.ToString("F5") + ", " + position.y.ToString("F5") + ")",
            //     "    mov: (" + toControlPosition.x.ToString("F5") + ", " + toControlPosition.y.ToString("F5") + ")",
            //     "    fix: (" + fixedPosition.x.ToString("F5") + ", " + fixedPosition.y.ToString("F5") + ")",
            //     "    ret: " + isMovingToControlPositionAllowed
            // }));

            // 曲がり角までにブロックなどがあり、阻まれている場合は移動方向を変更できない
            if (isMovingToControlPositionAllowed) {
                moveControlStatus.SetControl(MoveControl.None);
                moveStatus.SetDirection(controlDirection);
                moveStatus.SetSpeed(3f);
            }
            positionStatus.SetPosition(fixedPosition);
        }

        /// <summary>
        /// 操作なしの移動を適用する
        /// </summary>
        private void ApplyMove(Vector2 position, Vector2 move) {
            // positionからmoveの方向に、移動可能な場所まで移動する
            MoveChecker.CheckMoveValid(map.GetMapTiles(), position, move, out var fixedPosition);
            positionStatus.SetPosition(fixedPosition);

            // Debug.Log(string.Join("\n", new string[] {
            //     "[CheckMoveValid]",
            //     "    pos: (" + position.x.ToString("F5") + ", " + position.y.ToString("F5") + ")",
            //     "    mov: (" + move.x.ToString("F5") + ", " + move.y.ToString("F5") + ")",
            //     "    fix: (" + fixedPosition.x.ToString("F5") + ", " + fixedPosition.y.ToString("F5") + ")",
            // }));
        }

    }
}
