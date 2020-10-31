using System.Collections.Generic;
using UnityEngine;

namespace Hackman.Game.Map
{
    [CreateAssetMenu(fileName = "GameMap", menuName = "Hackman/Game/GameMap", order = 0)]
    public class GameMap : ScriptableObject
    {
        public string ResourcePath;
        public Vector2 StartPlayerPosition;
        public Vector2 RespawnPlayerPosition;
        public List<Vector2> MonstersPosition;
        public List<Vector2> KeysPosition;
        public Vector2 GoalPosition;
    }
}