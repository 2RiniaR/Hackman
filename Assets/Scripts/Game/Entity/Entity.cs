using UnityEngine;
using System;
using UniRx;

namespace Hackman.Game.Entity {
    public class Entity : MonoBehaviour
    {

        [SerializeField] protected Vector2Int initialDirection;

        [Header("移動")]
        public float moveSpeed;

        [SerializeField]
        protected Map.MapSystem map;

        [SerializeField]
        protected AnimatorParameter animatorParameter;

        protected MoveUpdater moveUpdater;
        protected MoveStatus moveStatus;
        protected MoveControlStatus moveControlStatus;
        protected AnimationUpdater animationUpdater;
        protected MoveSpeedStore moveSpeedStore;

        public Vector2 Direction => moveStatus.Direction;
        public float Speed => moveStatus.Speed;
        public MoveControl Control => moveControlStatus.Control;

        public IObservable<MoveControl> OnControlChanged => moveControlStatus.OnControlChanged;
        public IObservable<float> OnSpeedChanged => moveStatus.OnSpeedChanged;
        public IObservable<Vector2> OnDirectionChanged => moveStatus.OnDirectionChanged;

        protected virtual void Awake() {
            moveStatus = new MoveStatus();
            moveControlStatus = new MoveControlStatus();
            moveSpeedStore = new MoveSpeedStore(moveSpeed);
            moveUpdater = new MoveUpdater(moveControlStatus, transform, moveStatus, moveSpeedStore, map);
            animationUpdater = new AnimationUpdater(animatorParameter, moveStatus);
            moveStatus.SetDirection(initialDirection);
        }

        protected virtual void Start() {
            moveStatus.SetSpeed(0);
            moveControlStatus.SetControl(MoveControl.None);
        }

        protected virtual void OnDestroy() {
            animationUpdater.Dispose();
        }

        public void SetControl(MoveControl control) {
            moveControlStatus.SetControl(control);
        }

        protected virtual void Update() {
            moveUpdater.UpdatePosition();
        }

    }
}
