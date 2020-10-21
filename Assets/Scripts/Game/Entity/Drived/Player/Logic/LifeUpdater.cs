using UnityEngine;
using Hackman.Game.Map;
using System;
using System.Linq;
using UniRx;

namespace Hackman.Game.Entity.Player {
    public class LifeUpdater : IDisposable {

        private readonly CompositeDisposable onDispose = new CompositeDisposable();
        private readonly LifeStatus lifeStatus;

        public LifeUpdater(IObservable<Unit> onKilledObservable, LifeStatus lifeStatus) {
            this.lifeStatus = lifeStatus;
            onKilledObservable.Subscribe(_ => ReduceLife()).AddTo(onDispose);
        }

        public void Dispose() {
            onDispose.Dispose();
        }

        private void ReduceLife() {
            lifeStatus.SetLife(lifeStatus.Life - 1);
        }

    }
}
