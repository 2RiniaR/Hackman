using System;
using Hackman.Game.Map;
using UnityEngine;

namespace Hackman.Game.Entity
{
    public class Entity : MonoBehaviour
    {
        [Header("移動")] public float moveSpeed;

        [SerializeField] protected AnimatorParameter animatorParameter;

        private AnimationUpdater _animationUpdater;

        private MoveSpeedStore _moveSpeedStore;
        private MoveStatus _moveStatus;

        private MoveUpdater _moveUpdater;
        private MapSystem _map;
        protected MoveControlStatus MoveControlStatus;

        public Vector2 Direction => _moveStatus.Direction;

        protected virtual void Awake()
        {
            _map = FindObjectOfType<MapSystem>();
            _moveStatus = new MoveStatus();
            MoveControlStatus = new MoveControlStatus();
            _moveSpeedStore = new MoveSpeedStore(moveSpeed);
            _moveUpdater = new MoveUpdater(MoveControlStatus, transform, _moveStatus, _moveSpeedStore, _map);
            _animationUpdater = new AnimationUpdater(animatorParameter, _moveStatus);
        }

        protected virtual void Start()
        {
            _moveStatus.SetSpeed(0);
            SetControl(MoveControl.None);
        }

        protected virtual void Update()
        {
            _moveUpdater.UpdatePosition();
        }

        protected virtual void OnDestroy()
        {
            _animationUpdater.Dispose();
        }

        public void SetControl(MoveControl control)
        {
            MoveControlStatus.SetControl(control);
        }
    }
}