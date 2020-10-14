using UnityEngine;
using UniRx;
using System;

namespace Hackman.Game.Player {
    public class PositionStatus {

        private readonly ReactiveProperty<Vector2> position = new ReactiveProperty<Vector2>();
        public Vector2 Position => position.Value;
        public IObservable<Vector2> OnPositionChanged => position;

        public void Move(Vector2 vec) {
            position.Value += vec;
        }

        public void SetPosition(Vector2 position) {
            this.position.Value = position;
        }

    }
}
