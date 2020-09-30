using UnityEngine;
using UniRx;
using System;

namespace Hackman.Game.Player {
    public class MoveStatus {
        private readonly ReactiveProperty<Vector2> velocity = new ReactiveProperty<Vector2>();
        public Vector2 Velocity => velocity.Value;
        public IObservable<Vector2> OnVelocityChanged => velocity;
    }
}
