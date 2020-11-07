using System;

namespace Game.Entity.Player
{
    public interface IInputControl : IDisposable
    {
        void SetEnable(bool isEnable);
    }
}