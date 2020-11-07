using System;

namespace Game.System.Phase.Elements
{
    public abstract class PhaseElement : IDisposable
    {
        public virtual void Dispose()
        {
        }

        public abstract void Activate();
        public abstract void Deactivate();
    }
}