using UnityEngine;
using System;

namespace Hackman.Game.Entity.Player {
    public class Player : Entity {

        private IInputControl inputControl;

        protected override void Awake() {
            base.Awake();
            inputControl = new ButtonInputControl(moveControlStatus);
        }

        protected override void OnDestroy() {
            base.Awake();
            inputControl.Dispose();
        }

    }
}
