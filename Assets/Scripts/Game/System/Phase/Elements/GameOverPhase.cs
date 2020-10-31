using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Hackman.Game.Phase {
    public class GameOverPhase : PhaseElement {

        private readonly DebugInput _input;
        private const string StartGameSceneName = "StartScene";

        public GameOverPhase()
        {
            _input = new DebugInput();
            _input.UI.Submit.started += OnSubmit;
        }

        public override void Activate() {
            var gameOverAnimation = Object.FindObjectOfType<GameOverAnimation>();
            gameOverAnimation.Play(() => {
                _input.Enable();
            });
        }

        public override void Deactivate() {

        }

        private void OnSubmit(InputAction.CallbackContext context)
        {
            SceneManager.LoadScene(StartGameSceneName);
        }

        public override void Dispose()
        {
            _input.UI.Submit.started -= OnSubmit;
        }
    }
}
