using Game.View;
using Hackman;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.System.Phase.Elements
{
    public class GameClearPhase : PhaseElement
    {
        private const string StartGameSceneName = "StartScene";

        private readonly DebugInput _input;

        public GameClearPhase()
        {
            _input = new DebugInput();
            _input.UI.Submit.started += OnSubmit;
        }

        public override void Activate()
        {
            var gameClearAnimation = Object.FindObjectOfType<GameClearAnimation>();
            gameClearAnimation.Play(() => { _input.Enable(); });
        }

        public override void Deactivate()
        {
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