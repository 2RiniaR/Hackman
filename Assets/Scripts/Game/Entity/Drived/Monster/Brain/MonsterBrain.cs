using UnityEngine;
using System.Collections.Generic;

namespace Hackman.Game.Entity.Monster.Brain {
    public class MonsterBrain : MonoBehaviour {

        [SerializeField]
        private Player.Player player;

        [SerializeField]
        private List<Monster> monsters;

        [SerializeField]
        private Map.MapSystem mapSystem;

        private void Start() {
            var graph = MapGraphConverter.GetGraph(mapSystem.Field);
            Debug.Log(string.Join("\n", new string[] {
                "[MapGraph]",
                graph.ToString()
            }));
        }

    }
}
