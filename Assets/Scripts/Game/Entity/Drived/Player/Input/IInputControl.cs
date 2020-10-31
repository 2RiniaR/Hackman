using System;

namespace Hackman.Game.Entity.Player {
    public interface IInputControl : IDisposable
    {
        void SetEnable(bool isEnable);
    }
}
