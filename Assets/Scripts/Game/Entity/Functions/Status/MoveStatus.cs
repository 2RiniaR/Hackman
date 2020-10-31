using UnityEngine;
using UniRx;
using System;

namespace Hackman.Game.Entity {
    public class MoveStatus {

        private readonly ReactiveProperty<Vector2> _direction = new ReactiveProperty<Vector2>();
        private readonly FloatReactiveProperty _speed = new FloatReactiveProperty();
        public Vector2 Direction => _direction.Value;
        public float Speed => _speed.Value;
        public IObservable<Vector2> OnDirectionChanged => _direction;
        public IObservable<float> OnSpeedChanged => _speed;

        public void SetStop() {
            SetSpeed(0f);
        }

        public void SetDirection(Vector2 direction) {
            this._direction.Value = direction.normalized;
        }

        public void SetSpeed(float speed) {
            this._speed.Value = speed;
        }

        public Vector2 GetFlameMoveVector() {
            return Direction * Speed * Time.deltaTime;
        }

    }
}
