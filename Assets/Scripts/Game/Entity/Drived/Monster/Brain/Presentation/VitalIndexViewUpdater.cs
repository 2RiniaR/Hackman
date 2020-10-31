using System;
using Hackman.Game.Entity.Player;
using UniRx;

namespace Hackman.Game.Entity.Monster.Brain.Presentation
{
    public class VitalIndexViewUpdater : IDisposable
    {
        private readonly CompositeDisposable _onDispose = new CompositeDisposable();
        private readonly VitalIndexView _vitalIndexView;

        public VitalIndexViewUpdater(VitalIndexView vitalIndexView) {
            _vitalIndexView = vitalIndexView;
        }

        public void Dispose() {
            _onDispose.Dispose();
        }

        private void UpdateDisplay(float vitalIndex) {
        }
    }
}