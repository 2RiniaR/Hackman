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

        private float _debugTimer;

        private void Start()
        {
            StartCoroutine(DebugInterval());
        }

        private IEnumerator DebugInterval()
        {
            while (true)
            {
                _debugTimer -= Time.deltaTime;
                if (_debugTimer <= 0f)
                {
                    DebugSurvivalIndex();
                    _debugTimer = 2f;
                }

                yield return null;
            }
        }

        private void DebugSurvivalIndex()
        {
            var mapGraphConverter = new MapGraphConverter(mapSystem.Field);
            var surviveIndex = SurvivalIndexCalculator.GetSurvivalIndex(
                new EntityStatus(player.transform.localPosition - new Vector3(.5f, .5f), player.Direction),
                monsters.Select(m => new EntityStatus(m.transform.localPosition - new Vector3(.5f, .5f), m.Direction)),
                mapGraphConverter.GetGraph());
            Debug.Log("生存指数: " + surviveIndex);
        }
    }
}