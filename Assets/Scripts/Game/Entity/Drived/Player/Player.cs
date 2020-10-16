using UnityEngine;
using System;

namespace Hackman.Game.Entity.Player {
    public class Player : Entity {

        private IInputControl inputControl;
        private DotEater dotEater;

        protected override void Awake() {
            base.Awake();
            inputControl = new ButtonInputControl(moveControlStatus);
            dotEater = new DotEater(map, moveUpdater);
        }

        protected override void OnDestroy() {
            dotEater.Dispose();
            inputControl.Dispose();
            base.OnDestroy();
        }

    }
}
