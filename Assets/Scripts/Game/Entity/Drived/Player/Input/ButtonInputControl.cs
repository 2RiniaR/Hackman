using UnityEngine;
using UnityEngine.InputSystem;

namespace Hackman.Game.Entity.Player {
    public class ButtonInputControl : IInputControl {

        private readonly DebugInput _input = new DebugInput();
        private readonly MoveControlStatus _moveControl;

        public ButtonInputControl(MoveControlStatus moveControl) {
            _moveControl = moveControl;
            _input.Player.Move.started += OnMoveControl;
        }

        public void Dispose() {
            _input.Player.Move.started -= OnMoveControl;
        }

        public void SetEnable(bool isEnable)
        {
            if (isEnable) _input.Enable();
            else _input.Disable();
        }

        private void OnMoveControl(InputAction.CallbackContext context) {
            var direction = context.ReadValue<Vector2>();
            if (direction.x > 0) {
                _moveControl.SetControl(MoveControl.DirectionRight);
            } else if (direction.x < 0) {
                _moveControl.SetControl(MoveControl.DirectionLeft);
            } else if (direction.y > 0) {
                _moveControl.SetControl(MoveControl.DirectionUp);
            } else if (direction.y < 0) {
                _moveControl.SetControl(MoveControl.DirectionDown);
            }
        }

    }
}
