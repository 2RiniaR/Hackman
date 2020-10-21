using UnityEngine;
using System;
using UniRx;

namespace Hackman.Game.Entity.Player {
    public class KilledAnimationPlayer : IDisposable {

        private readonly CompositeDisposable onDispose = new CompositeDisposable();
        private readonly PlayerLifeDisplay lifeDisplay;

        public KilledAnimationPlayer(PlayerLifeDisplay lifeDisplay, LifeStatus lifeStatus) {
            this.lifeDisplay = lifeDisplay;
            lifeStatus.OnLifeChanged.Subscribe(UpdateDisplay).AddTo(onDispose);
        }

        public void Dispose() {
            onDispose.Dispose();
        }

        private void UpdateDisplay(int life) {
            lifeDisplay.SetLifeCount(life);
        }

    }
}
