using System.Collections.Generic;
using UnityEngine;

namespace Game.System.Map
{
    [CreateAssetMenu(fileName = "GameMap", menuName = "Hackman/Game/GameMap", order = 0)]
    public class GameMap : ScriptableObject
    {
        public string resourcePath;
        public Vector2 startPlayerPosition;
        public Vector2 respawnPlayerPosition;
        public List<Vector2> monstersPosition;
        public List<Vector2> keysPosition;
        public Vector2 goalPosition;
    }
}