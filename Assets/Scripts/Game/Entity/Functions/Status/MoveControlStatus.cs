using UniRx;
using System;

namespace Hackman.Game.Entity {
    public class MoveControlStatus {

        private readonly ReactiveProperty<MoveControl> control = new ReactiveProperty<MoveControl>();
        public MoveControl Control => control.Value;
        public IObservable<MoveControl> OnControlChanged => control;

        public void SetControl(MoveControl control) {
            this.control.Value = control;
        }

    }
}
