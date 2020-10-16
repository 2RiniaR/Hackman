using UnityEngine;
using System;

namespace Hackman.Game.Entity {
    public class Entity : MonoBehaviour {

        [SerializeField]
        private Map.MapSystem map;

        [Header("移動")]
        public float moveSpeed;

        [SerializeField]
        private Vector2 initialPosition;

        [SerializeField]
        private AnimatorParameter animatorParameter;

        private MoveUpdater moveUpdater;
        private MoveStatus moveStatus;
        private PositionStatus positionStatus;
        private MoveControlStatus moveControlStatus;
        private AnimationUpdater animationUpdater;
        private DrawPositionUpdater drawPositionUpdater;

        public IObservable<MoveControl> OnControlChanged => moveControlStatus.OnControlChanged;
        public IObservable<float> OnSpeedChanged => moveStatus.OnSpeedChanged;
        public IObservable<Vector2> OnDirectionChanged => moveStatus.OnDirectionChanged;
        public IObservable<Vector2> OnPositionChanged => positionStatus.OnPositionChanged;

        private void Awake() {
            moveStatus = new MoveStatus();
            positionStatus = new PositionStatus();
            moveControlStatus = new MoveControlStatus();
            moveUpdater = new MoveUpdater(moveControlStatus, positionStatus, moveStatus, map);
            animationUpdater = new AnimationUpdater(animatorParameter, moveStatus);
            drawPositionUpdater = new DrawPositionUpdater(transform, positionStatus);
        }

        private void Start() {
            positionStatus.SetPosition(initialPosition);
            moveStatus.SetSpeed(0);
            moveControlStatus.SetControl(MoveControl.None);
        }

        private void OnDestroy() {
            moveUpdater.Dispose();
            animationUpdater.Dispose();
            drawPositionUpdater.Dispose();
        }

    }
}
