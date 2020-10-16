using UnityEngine;
using System;

namespace Hackman.Game.Entity.Player {
    public class Player : Entity {

        [Header("移動")]
        public float moveSpeed;

        private IInputControl inputControl;
        private MoveSpeedStore moveSpeedStore;

        protected override void Awake() {
            base.Awake();
            moveSpeedStore = new MoveSpeedStore(moveSpeed);
            inputControl = new ButtonInputControl(moveControlStatus);
        }

        protected override void OnDestroy() {
            base.Awake();
            inputControl.Dispose();
        }

    }
}
