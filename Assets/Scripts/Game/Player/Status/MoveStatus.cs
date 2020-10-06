using UnityEngine;
using UniRx;
using System;

namespace Hackman.Game.Player {
    public class MoveStatus {

        private readonly ReactiveProperty<Vector2> velocity = new ReactiveProperty<Vector2>();
        public Vector2 Velocity => velocity.Value;
        public IObservable<Vector2> OnVelocityChanged => velocity;

        public void SetStop() {
            SetVelocity(Vector2.zero);
        }

        public void SetVelocity(Vector2 velocity) {
            this.velocity.Value = velocity;
        }

        public void SetDirection(Vector2 direction) {
            velocity.Value = direction * velocity.Value.magnitude;
        }

        public void SetSpeed(float speed) {
            velocity.Value = velocity.Value.normalized * speed;
        }

    }
}
