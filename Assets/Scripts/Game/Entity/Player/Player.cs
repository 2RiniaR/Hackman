using System;

namespace Game.Entity.Player
{
    public class Player : Entity
    {
        private CollisionDetector _collisionDetector;
        private IInputControl _inputControl;
        public IObservable<Entity> OnCollision => _collisionDetector.OnCollision;

        protected override void Awake()
        {
            base.Awake();
            _inputControl = new ButtonInputControl(this);
            _collisionDetector = new CollisionDetector(transform);
        }

        protected override void Update()
        {
            base.Update();
            _collisionDetector.CheckCollision();
        }

        private void OnEnable()
        {
            _inputControl.SetEnable(true);
        }

        private void OnDisable()
        {
            _inputControl.SetEnable(false);
        }

        protected void OnDestroy()
        {
            _inputControl.Dispose();
        }
    }
}