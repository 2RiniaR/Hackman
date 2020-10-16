using UnityEngine;
using System;

namespace Hackman.Game.Entity.Monster {
    public class Monster : Entity {

        [Header("移動")]
        public float moveSpeed;

        private MoveSpeedStore moveSpeedStore;

        protected override void Awake() {
            base.Awake();
            moveSpeedStore = new MoveSpeedStore(moveSpeed);
        }

        protected override void OnDestroy() {
            base.Awake();
        }

    }
}
