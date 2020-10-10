using UniRx;
using System;
using UnityEngine;

namespace Hackman.Game.Player {
    public class MoveUpdater : IDisposable {

        private CompositeDisposable onDispose = new CompositeDisposable();
        private readonly PositionStatus position;
        private readonly MoveStatus move;

        public MoveUpdater(PositionStatus position, MoveStatus move) {
            this.position = position;
            this.move = move;
            Observable.EveryUpdate().Subscribe(_ => Move()).AddTo(onDispose);
        }

        public void Dispose() {
            onDispose.Dispose();
        }

        private void Move() {
            position.Move(move.Velocity * Time.deltaTime);
        }

    }
}
