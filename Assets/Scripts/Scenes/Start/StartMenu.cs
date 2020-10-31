using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Hackman.Scenes.Start
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private string mainGameSceneName = "";
        private DebugInput _input;

        private void Awake()
        {
            _input = new DebugInput();
            _input.Enable();
        }

        private void Start()
        {
            _input.UI.Submit.performed += OnSubmit;
        }

        private void OnDestroy()
        {
            _input.UI.Submit.performed -= OnSubmit;
        }

        private void OnSubmit(InputAction.CallbackContext context)
        {
            SceneManager.LoadScene(mainGameSceneName);
        }
    }
}