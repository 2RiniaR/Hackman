using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hackman.Game.Map;
using UnityEngine;

namespace Hackman.Game.Entity.Monster.Brain
{
    public class MonsterBrain : MonoBehaviour
    {
        [SerializeField] private Player.Player player;

        [SerializeField] private List<Monster> monsters;

        [SerializeField] private MapSystem mapSystem;

        private MapGraph _mapGraph;

        private void Start()
        {
            var mapGraphConverter = new MapGraphConverter(mapSystem.Field);
            _mapGraph = mapGraphConverter.GetGraph();
        }

        private void Update()
        {
            DebugSurvivalIndex();
        }

        private void DebugSurvivalIndex()
        {
            var playerStatus =
                new EntityStatus(player.transform.localPosition - new Vector3(.5f, .5f), player.Direction);
            var monstersStatus = monsters.Select(m =>
                new EntityStatus(m.transform.localPosition - new Vector3(.5f, .5f), m.Direction));
            var surviveIndex = SurvivalIndexCalculator.GetSurvivalIndex(playerStatus, monstersStatus, _mapGraph);
            Debug.Log("生存指数: " + surviveIndex + "\n[Player] pos: " + playerStatus.Position + " dir: " +
                      playerStatus.Direction + "\n" + monstersStatus.Select((m, i) =>
                          "[Monster" + i + "] pos: " + m.Position + " dir: " + m.Direction + "\n"));
        }
    }
}