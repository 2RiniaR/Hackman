using System;

namespace Hackman.Game.Phase {
    public abstract class PhaseElement : IDisposable {
        public abstract void Activate();
        public abstract void Deactivate();
        public virtual void Dispose() {}
    }
}
