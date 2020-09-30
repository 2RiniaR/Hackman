using UnityEngine;

namespace Hackman.Game.Player {
    public class Player : MonoBehaviour {

        [Header("操作")]
        public ControlButtonSetting controlButtonSetting;

        [Header("移動")]
        public float moveSpeed;

        [SerializeField]
        private Rigidbody2D _rigidbody;

        private IInputControl inputControl;
        private MoveUpdater moveUpdater;
        private MoveSimulator moveSimulator;
        private MoveStatus moveStatus;
        private MoveSpeedStore moveSpeedStore;

        private void Awake() {
            moveStatus = new MoveStatus();
            moveSimulator = new MoveSimulator(_rigidbody);
            moveSpeedStore = new MoveSpeedStore(moveSpeed);
            moveUpdater = new MoveUpdater(moveSimulator, moveStatus);
            inputControl = new ButtonInputControl(controlButtonSetting);
        }

        private void OnDestroy() {
            moveUpdater.Dispose();
            inputControl.Dispose();
        }

    }
}
