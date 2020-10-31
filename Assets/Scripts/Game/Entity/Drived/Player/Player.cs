using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Hackman.Game.Entity.Player
{
    public class Player : Entity
    {
        private IInputControl _inputControl;
        private CollisionDetector _collisionDetector;
        public IObservable<Entity> OnCollision => _collisionDetector.OnCollision;

        protected override void Awake()
        {
            base.Awake();
            _inputControl = new ButtonInputControl(MoveControlStatus);
            _collisionDetector = new CollisionDetector(transform);
        }

        private void OnEnable()
        {
            _inputControl.SetEnable(true);
        }

        private void OnDisable()
        {
            _inputControl.SetEnable(false);
        }

        protected override void Update()
        {
            base.Update();
            _collisionDetector.CheckCollision();
        }

        protected override void OnDestroy()
        {
            _inputControl.Dispose();
            base.OnDestroy();
        }
    }
}