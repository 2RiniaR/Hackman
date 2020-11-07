using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Game.Entity.Monster.Brain.Map;
using Game.Entity.Monster.Brain.Presentation;
using Game.System.Map;
using Game.View;
using UnityEngine;

namespace Game.Entity.Monster.Brain
{
    public class MonsterBrain : MonoBehaviour
    {

        [SerializeField] private Player.Player player;

        [SerializeField] private MapSystem mapSystem;

        [SerializeField] private VitalIndexView vitalIndexView;

        private readonly List<Monster> _monsters = new List<Monster>();

        private MapGraph _mapGraph;
        private VitalIndexViewUpdater _vitalIndexViewUpdater;

        private void Awake()
        {
            _vitalIndexViewUpdater = new VitalIndexViewUpdater(vitalIndexView);
        }

        private void Start()
        {
            _mapGraph = MapGraph.FromMapField(mapSystem.Field.Value);
            foreach (var monster in FindObjectsOfType<Monster>())
                _monsters.Add(monster);
        }

        private static EntityStatus GetEntityStatus(Entity entity)
        {
            return new EntityStatus(entity.GetEntityPosition(), entity.CurrentDirection);
        }

        private void Update()
        {
            /*
            // 操作の選択肢を取得
            var playerStatus = GetEntityStatus(player);
            var monstersStatus = _monsters.Select(GetEntityStatus).ToArray();
            var controlChances = ControlChanceDetector.Detect(_mapGraph, monstersStatus)
                .Where(cc => _monsters[cc.MonsterID].controlCoolTimeLeft <= 0f);

            // 操作の選択肢がなければ終了
            var controlChancesArray = controlChances as ControlChanceDetector.ControlChance[] ?? controlChances.ToArray();
            if (!controlChancesArray.Any()) return;

            // すべての操作パターンを列挙する
            var firstMonster = controlChancesArray.First();
            var firstMonsterID = firstMonster.MonsterID;
            var allControlPatterns = firstMonster.DirectionChoices.Select(
                    dir => new MonsterControl {MonsterID = firstMonsterID, Direction = dir}
                )
                .Select(control => new MonsterControlPattern {Controls = new[] {control}});

            foreach (var controlChance in controlChancesArray.Skip(1))
            {
                var newAllControlPatterns = new List<MonsterControlPattern>();
                // controlChance: 各モンスターの「移動可能方向の配列」と「モンスターのID」
                // controlPatterns: 各モンスターの「移動可能方向」と「モンスターのID」
                var controlPatterns = controlChance.DirectionChoices.Select(
                    dir => new MonsterControl {MonsterID = controlChance.MonsterID, Direction = dir}
                ).ToArray();

                foreach (var alreadyExistControlPattern in allControlPatterns)
                foreach (var controlPattern in controlPatterns)
                {
                    var newPattern = new MonsterControlPattern
                    {
                        Controls = alreadyExistControlPattern.Controls.Concat(new[] {controlPattern}).ToArray()
                    };
                    newAllControlPatterns.Add(newPattern);
                }

                allControlPatterns = newAllControlPatterns;
            }

            // 生存指数が最小となる選択肢を見つける
            var allControlPatternsArray = allControlPatterns as MonsterControlPattern[] ?? allControlPatterns.ToArray();
            var minVitalIndex = float.MaxValue;
            var optimalControlPatternIndex = 0;
            for (var i = 0; i < allControlPatternsArray.Length; i++)
            {
                // モンスターのステータスを仮定する
                var controlPattern = allControlPatternsArray[i];
                var monstersStatusPattern = _monsters.Select(m =>
                    new EntityStatus(m.transform.localPosition - new Vector3(.5f, .5f), m.Direction)).ToArray();
                foreach (var control in controlPattern.Controls)
                    monstersStatusPattern[control.MonsterID] =
                        new EntityStatus(monstersStatusPattern[control.MonsterID].Position, control.Direction);

                // 生存指数を求めて、最小ならば入れ替える
                var vitalIndex =
                    SurvivalIndexCalculator.GetSurvivalIndex(playerStatus, monstersStatusPattern, _mapGraph);
                if (!(vitalIndex < minVitalIndex)) continue;
                minVitalIndex = vitalIndex;
                optimalControlPatternIndex = i;
            }

            // モンスターに操作を伝える
            var optimalControlPattern = allControlPatternsArray[optimalControlPatternIndex];
            foreach (var control in optimalControlPattern.Controls)
            {
                if (!VectorControlMap.ContainsKey(control.Direction)) continue;
                _monsters[control.MonsterID].SetControl(VectorControlMap[control.Direction]);
                _monsters[control.MonsterID].controlCoolTimeLeft = Monster.ControlCoolTime;
            }
            */
        }

        private void OnDestroy()
        {
            _vitalIndexViewUpdater.Dispose();
        }

        private struct MonsterControl
        {
            public int MonsterID;
            public Vector2Int Direction;
        }

        private struct MonsterControlPattern
        {
            public MonsterControl[] Controls;
        }
    }
}