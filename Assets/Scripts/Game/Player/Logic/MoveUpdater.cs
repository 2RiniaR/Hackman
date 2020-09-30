using UniRx;
using System;

namespace Hackman.Game.Player {
    public class MoveUpdater : IDisposable {

        private CompositeDisposable onDispose = new CompositeDisposable();
        private readonly MoveSimulator simulator;
        private readonly MoveStatus status;

        public MoveUpdater(MoveSimulator simulator, MoveStatus status) {
            this.simulator = simulator;
            this.status = status;
            Observable.EveryUpdate().Subscribe(_ => Move()).AddTo(onDispose);
        }

        public void Dispose() {
            onDispose.Dispose();
        }

        private void Move() {
            simulator.Move(status.Velocity);
        }

    }
}
