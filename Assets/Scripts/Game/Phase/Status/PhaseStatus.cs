using System;
using UniRx;

namespace Hackman.Game.Phase {
    public class PhaseStatus {

        private readonly ReactiveProperty<Phase> phase = new ReactiveProperty<Phase>();
        public Phase Phase => phase.Value;
        public IObservable<Phase> OnPhaseChanged => phase;

        public void SetPhase(Phase phase) {
            this.phase.Value = phase;
        }

    }
}
