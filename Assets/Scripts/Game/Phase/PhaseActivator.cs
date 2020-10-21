using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hackman.Game.Phase {
    public class PhaseActivator {

        private readonly ReadOnlyDictionary<Phase, PhaseElement> elements = new ReadOnlyDictionary<Phase, PhaseElement>(
            new Dictionary<Phase, PhaseElement>() {
                { Phase.Start,       new StartPhase()       },
                { Phase.Active,      new ActivePhase()      },
                { Phase.PlayerDeath, new PlayerDeathPhase() },
            }
        );

        private readonly PhaseStatus phaseStatus = new PhaseStatus();

        public void SetPhase(Phase phase) {
            if (phaseStatus.Phase == phase) {
                return;
            }
            if (elements.TryGetValue(phaseStatus.Phase, out var previousElement)) {
                previousElement.Deactivate();
            }
            if (elements.TryGetValue(phase, out var afterElement)) {
                afterElement.Activate();
            }
            phaseStatus.SetPhase(phase);
        }

    }
}
