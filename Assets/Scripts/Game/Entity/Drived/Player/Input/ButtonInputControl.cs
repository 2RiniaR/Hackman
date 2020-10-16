using UnityEngine;
using UnityEngine.InputSystem;

namespace Hackman.Game.Entity.Player {
    public class ButtonInputControl : IInputControl {

        private DebugInput input = new DebugInput();
        private readonly MoveControlStatus moveControl;

        public ButtonInputControl(MoveControlStatus moveControl) {
            this.moveControl = moveControl;
            input.Player.Move.started += OnMoveControl;
            input.Enable();
        }

        public void Dispose() {
            input.Disable();
            input.Player.Move.started -= OnMoveControl;
        }

        private void OnMoveControl(InputAction.CallbackContext context) {
            Vector2 direction = context.ReadValue<Vector2>();
            if (direction.x > 0) {
                moveControl.SetControl(MoveControl.DirectionRight);
            } else if (direction.x < 0) {
                moveControl.SetControl(MoveControl.DirectionLeft);
            } else if (direction.y > 0) {
                moveControl.SetControl(MoveControl.DirectionUp);
            } else if (direction.y < 0) {
                moveControl.SetControl(MoveControl.DirectionDown);
            }
        }

    }
}
