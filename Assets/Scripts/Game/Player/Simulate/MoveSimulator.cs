using UnityEngine;

namespace Hackman.Game.Player {
    public class MoveSimulator {

        private readonly Rigidbody2D rigidbody;

        public MoveSimulator(Rigidbody2D rigidbody) {
            this.rigidbody = rigidbody;
        }

        public void Move(Vector2 vec) {
            rigidbody.velocity = vec;
        }

    }
}
