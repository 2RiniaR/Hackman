using UnityEngine;

namespace Hackman.Game.Player {
    public class Player : MonoBehaviour {

        [SerializeField]
        private Map.MapSystem map;

        [Header("移動")]
        public float moveSpeed;

        private IInputControl inputControl;
        private MoveUpdater moveUpdater;
        private MoveStatus moveStatus;
        private PositionStatus positionStatus;
        private MoveSpeedStore moveSpeedStore;

        private void Awake() {
            moveStatus = new MoveStatus();
            positionStatus = new PositionStatus();
            moveSpeedStore = new MoveSpeedStore(moveSpeed);
            moveUpdater = new MoveUpdater(positionStatus, moveStatus);
            inputControl = new ButtonInputControl(moveStatus, moveSpeedStore);
        }

        private void OnDestroy() {
            moveUpdater.Dispose();
            inputControl.Dispose();
        }

    }
}
