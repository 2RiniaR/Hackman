using System;
using Game.View;
using UniRx;
using UnityEngine;

namespace Game.Entity.Monster.Brain.Presentation
{
    public class VitalIndexViewUpdater : IDisposable
    {
        private const float MAXDisplayValue = 25f;
        private readonly CompositeDisposable _onDispose = new CompositeDisposable();
        private readonly VitalIndexView _vitalIndexView;

        public VitalIndexViewUpdater(VitalIndexView vitalIndexView)
        {
            _vitalIndexView = vitalIndexView;
        }

        public void Dispose()
        {
            _onDispose.Dispose();
        }

        public void UpdateDisplay(float vitalIndex)
        {
            _vitalIndexView.SetValue(Mathf.Clamp(vitalIndex, 0f, MAXDisplayValue) / MAXDisplayValue);
        }
    }
}