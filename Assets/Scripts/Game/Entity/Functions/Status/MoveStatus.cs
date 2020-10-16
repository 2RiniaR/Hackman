using UnityEngine;
using UniRx;
using System;

namespace Hackman.Game.Entity {
    public class MoveStatus {

        private readonly ReactiveProperty<Vector2> direction = new ReactiveProperty<Vector2>();
        private readonly FloatReactiveProperty speed = new FloatReactiveProperty();
        public Vector2 Direction => direction.Value;
        public float Speed => speed.Value;
        public IObservable<Vector2> OnDirectionChanged => direction;
        public IObservable<float> OnSpeedChanged => speed;

        public void SetStop() {
            SetSpeed(0f);
        }

        public void SetDirection(Vector2 direction) {
            this.direction.Value = direction.normalized;
        }

        public void SetSpeed(float speed) {
            this.speed.Value = speed;
        }

        public Vector2 GetFlameMoveVector() {
            return Direction * Speed * Time.deltaTime;
        }

    }
}
