using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Hackman.Game.Phase {
    public class GameClearPhase : PhaseElement
    {

        private readonly DebugInput _input;
        private const string StartGameSceneName = "StartScene";

        public GameClearPhase()
        {
            _input = new DebugInput();
            _input.UI.Submit.started += OnSubmit;
        }

        public override void Activate() {
            var gameClearAnimation = Object.FindObjectOfType<GameClearAnimation>();
            gameClearAnimation.Play(() => {
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
