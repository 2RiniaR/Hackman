using Hackman.Game;
using Hackman.Game.Map;
using UnityEngine;

namespace Hackman.Scenes.MainGame
{
    public class GameActivator : MonoBehaviour
    {
        [SerializeField] private GameSystem gameSystem;
        [SerializeField] private GameMap gameMap;

        private void Start()
        {
            gameSystem.StartGame(gameMap);
        }
    }
}