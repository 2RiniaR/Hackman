using UnityEngine;
using UnityEngine.InputSystem;

namespace Hackman.Game.Player {
    public class ButtonInputControl : IInputControl {

        private DebugInput input = new DebugInput();

        public ButtonInputControl() {
            input.Player.Move.started += OnMove;
            input.Enable();
        }

        public void Dispose() {
            input.Disable();
            input.Player.Move.started -= OnMove;
        }

        private void OnMove(InputAction.CallbackContext context) {
            Debug.Log("move: " + context.ReadValue<Vector2>());
        }

    }
}
