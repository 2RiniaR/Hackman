using UnityEngine;

namespace Hackman.Game.Entity {
    public class MoveSpeedStore {

        private float moveSpeed;
        public float MoveSpeed {
            get { return moveSpeed; }
            set { moveSpeed = Mathf.Max(0f, value); }
        }

        public MoveSpeedStore(float value) {
            MoveSpeed = value;
        }

    }
}
