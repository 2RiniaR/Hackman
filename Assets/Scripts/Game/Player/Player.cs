using UnityEngine;
using System;

namespace Hackman.Game.Player {
    public class Player : MonoBehaviour {

        [SerializeField]
        private Map.MapSystem map;

        [Header("移動")]
        public float moveSpeed;

        [SerializeField]
        private Vector2 initialPosition;

        [SerializeField]
        private AnimatorParameter animatorParameter;

        private IInputControl inputControl;
        private MoveUpdater moveUpdater;
        private MoveStatus moveStatus;
        private PositionStatus positionStatus;
        private MoveControlStatus moveControlStatus;
        private MoveSpeedStore moveSpeedStore;
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
            moveSpeedStore = new MoveSpeedStore(moveSpeed);
            moveUpdater = new MoveUpdater(moveControlStatus, positionStatus, moveStatus, map);
            inputControl = new ButtonInputControl(moveControlStatus);
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
            inputControl.Dispose();
            animationUpdater.Dispose();
            drawPositionUpdater.Dispose();
        }

    }
}
