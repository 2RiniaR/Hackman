using Game.System;
using Game.System.Map;
using UniRx;
using UnityEngine;

namespace Game.Entity
{
    public class Entity : MonoBehaviour
    {
        public float moveSpeed;
        public readonly ReactiveProperty<EntityControl> CurrentControl = new ReactiveProperty<EntityControl>(EntityControl.None);
        public readonly ReactiveProperty<Action> CurrentAction = new ReactiveProperty<Action>(new Action(ActionPattern.Stop));
        public Direction CurrentDirection => CurrentAction.Value.GetDirection();
        public static readonly Vector2 Size = Vector2.one;
        public AnimatorParameter animatorParameter;

        private AnimationUpdater _animationUpdater;
        private MapSystem _map;
        private MoveUpdater _moveUpdater;

        protected virtual void Awake()
        {
            _map = FindObjectOfType<MapSystem>();
            _moveUpdater = new MoveUpdater(this, _map);
            _animationUpdater = new AnimationUpdater(this);
        }

        protected virtual void Update()
        {
            _moveUpdater.UpdatePosition();
        }

        public EntityPosition GetEntityPosition()
        {
            return EntityPosition.FromVector((Vector2)transform.localPosition - Size / 2f);
        }

        public void SetEntityPosition(EntityPosition position)
        {
            transform.localPosition = position.GetVector() + Size / 2f;
        }
    }
}