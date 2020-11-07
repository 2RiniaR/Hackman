using Hackman;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Entity.Player
{
    public class ButtonInputControl : IInputControl
    {
        private readonly DebugInput _input = new DebugInput();
        private readonly Entity _entity;

        public ButtonInputControl(Entity entity)
        {
            _entity = entity;
            _input.Player.Move.started += OnMoveControl;
        }

        public void Dispose()
        {
            _input.Player.Move.started -= OnMoveControl;
        }

        public void SetEnable(bool isEnable)
        {
            if (isEnable) _input.Enable();
            else _input.Disable();
        }

        private void OnMoveControl(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            if (direction.x > 0)
                _entity.CurrentControl.Value = new EntityControl(ControlPattern.DirectionRight);
            else if (direction.x < 0)
                _entity.CurrentControl.Value = new EntityControl(ControlPattern.DirectionLeft);
            else if (direction.y > 0)
                _entity.CurrentControl.Value = new EntityControl(ControlPattern.DirectionUp);
            else if (direction.y < 0)
                _entity.CurrentControl.Value = new EntityControl(ControlPattern.DirectionDown);
        }
    }
}