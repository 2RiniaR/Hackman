using Game.System;
using Game.System.Map;
using UnityEngine;

namespace Scenes.MainGame
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