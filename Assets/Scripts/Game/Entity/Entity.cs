using UnityEngine;
using System;

namespace Hackman.Game.Entity {
    public class Entity : MonoBehaviour {

        [SerializeField]
        protected Map.MapSystem map;

        [SerializeField]
        protected Vector2 initialPosition;

        [SerializeField]
        protected AnimatorParameter animatorParameter;

        protected MoveUpdater moveUpdater;
        protected MoveStatus moveStatus;
        protected PositionStatus positionStatus;
        protected MoveControlStatus moveControlStatus;
        protected AnimationUpdater animationUpdater;
        protected DrawPositionUpdater drawPositionUpdater;

        public Vector2 Position => positionStatus.Position;
        public Vector2 Direction => moveStatus.Direction;
        public float Speed => moveStatus.Speed;
        public MoveControl Control => moveControlStatus.Control;

        public IObservable<MoveControl> OnControlChanged => moveControlStatus.OnControlChanged;
        public IObservable<float> OnSpeedChanged => moveStatus.OnSpeedChanged;
        public IObservable<Vector2> OnDirectionChanged => moveStatus.OnDirectionChanged;
        public IObservable<Vector2> OnPositionChanged => positionStatus.OnPositionChanged;

        protected virtual void Awake() {
            moveStatus = new MoveStatus();
            positionStatus = new PositionStatus();
            moveControlStatus = new MoveControlStatus();
            moveUpdater = new MoveUpdater(moveControlStatus, positionStatus, moveStatus, map);
            animationUpdater = new AnimationUpdater(animatorParameter, moveStatus);
            drawPositionUpdater = new DrawPositionUpdater(transform, positionStatus);
        }

        protected virtual void Start() {
            positionStatus.SetPosition(initialPosition);
            moveStatus.SetSpeed(0);
            moveControlStatus.SetControl(MoveControl.None);
        }

        protected virtual void OnDestroy() {
            moveUpdater.Dispose();
            animationUpdater.Dispose();
            drawPositionUpdater.Dispose();
        }

        public void SetControl(MoveControl control) {
            moveControlStatus.SetControl(control);
        }

    }
}
