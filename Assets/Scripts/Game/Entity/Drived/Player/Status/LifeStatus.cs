using UnityEngine;
using System;
using UniRx;

namespace Hackman.Game.Entity.Player {
    public class LifeStatus {

        private readonly ReactiveProperty<int> life = new ReactiveProperty<int>();
        public int Life => life.Value;
        public IObservable<int> OnLifeChanged => life;

        public void SetLife(int life) {
            this.life.Value = life;
        }

    }
}
