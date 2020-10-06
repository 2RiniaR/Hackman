using UnityEngine;
using UnityEngine.InputSystem;

namespace Hackman.Game.Player {
    public class ButtonInputControl : IInputControl {

        private DebugInput input = new DebugInput();
        private readonly MoveStatus move;
        private readonly MoveSpeedStore speed;

        public ButtonInputControl(MoveStatus move, MoveSpeedStore speed) {
            this.move = move;
            this.speed = speed;
            input.Player.Move.started += OnMove;
            input.Enable();
        }

        public void Dispose() {
            input.Disable();
            input.Player.Move.started -= OnMove;
        }

        private void OnMove(InputAction.CallbackContext context) {
            Vector2 direction = context.ReadValue<Vector2>();
            float speed = this.speed.MoveSpeed;
            move.SetVelocity(direction * speed);
        }

    }
}
