namespace Hackman.Game.Entity.Player {
    public class MoveSpeedStore {

        private float moveSpeed;
        public float MoveSpeed {
            get {
                return moveSpeed;
            }
            set {
                moveSpeed = value;
            }
        }

        public MoveSpeedStore(float value) {
            MoveSpeed = value;
        }

    }
}
