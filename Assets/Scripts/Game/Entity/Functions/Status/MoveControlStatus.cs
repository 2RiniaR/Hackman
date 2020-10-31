using UniRx;
using System;

namespace Hackman.Game.Entity {
    public class MoveControlStatus {

        private readonly ReactiveProperty<MoveControl> _control = new ReactiveProperty<MoveControl>();
        public MoveControl Control => _control.Value;
        public IObservable<MoveControl> OnControlChanged => _control;

        public void SetControl(MoveControl control) {
            _control.Value = control;
        }

    }
}
