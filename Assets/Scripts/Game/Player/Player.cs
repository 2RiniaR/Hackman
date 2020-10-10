using UnityEngine;

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
        private MoveSpeedStore moveSpeedStore;
        private AnimationUpdater animationUpdater;
        private DrawPositionUpdater drawPositionUpdater;

        private void Awake() {
            moveStatus = new MoveStatus();
            positionStatus = new PositionStatus();
            moveSpeedStore = new MoveSpeedStore(moveSpeed);
            moveUpdater = new MoveUpdater(positionStatus, moveStatus);
            inputControl = new ButtonInputControl(moveStatus, moveSpeedStore);
            animationUpdater = new AnimationUpdater(animatorParameter, moveStatus);
            drawPositionUpdater = new DrawPositionUpdater(transform, positionStatus);
        }

        private void Start() {
            positionStatus.SetPosition(initialPosition);
        }

        private void OnDestroy() {
            moveUpdater.Dispose();
            inputControl.Dispose();
            animationUpdater.Dispose();
            drawPositionUpdater.Dispose();
        }

    }
}
