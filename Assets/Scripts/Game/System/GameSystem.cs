using Hackman.Game.DoorKey;
using Hackman.Game.Entity;
using Hackman.Game.Entity.Goal;
using Hackman.Game.Entity.Monster;
using Hackman.Game.Entity.Player;
using Hackman.Game.Map;
using Hackman.Game.Phase;
using Hackman.Game.PlayerLife;
using UniRx;
using UnityEngine;

namespace Hackman.Game
{
    public class GameSystem : MonoBehaviour
    {
        [SerializeField] private PhaseSystem phaseSystem;
        [SerializeField] private Player player;
        [SerializeField] private PlayerLifeSystem playerLifeSystem;
        [SerializeField] private DoorKeySystem doorKeySystem;
        [SerializeField] private MapSystem mapSystem;
        [SerializeField] private Monster monsterPrefab;
        [SerializeField] private Entity.DoorKey.DoorKey doorKeyPrefab;
        [SerializeField] private Goal goalPrefab;
        [SerializeField] private Transform entityParent;

        private Goal goal;

        private void Start()
        {
            player.OnCollision.Where(e => e is Monster).Subscribe(_ => OnPlayerDeath()).AddTo(this);
            player.OnCollision.Where(e => e is Entity.DoorKey.DoorKey)
                .Select(e => e as Entity.DoorKey.DoorKey)
                .Subscribe(OnPlayerGetDoorKey).AddTo(this);
            player.OnCollision.Where(e =>
            {
                var goal = e as Goal;
                return goal != null && goal.isOpened.Value;
            }).Subscribe(_ => OnCollisionToOpenedGoal()).AddTo(this);
        }

        /// <summary>
        ///     ゲームを開始する
        /// </summary>
        /// <param name="map">使用するマップ</param>
        public void StartGame(GameMap map)
        {
            gameObject.SetActive(false);
            Initialize(map);
            gameObject.SetActive(true);
        }

        /// <summary>
        ///     ゲームを初期化する
        /// </summary>
        /// <param name="map">使用するマップ</param>
        private void Initialize(GameMap map)
        {
            player.transform.localPosition = map.StartPlayerPosition + new Vector2(.5f, .5f);
            mapSystem.SetMap(map);
            foreach (var pos in map.MonstersPosition)
                Instantiate(monsterPrefab, pos + new Vector2(.5f, .5f), Quaternion.identity, entityParent);
            foreach (var pos in map.KeysPosition)
                Instantiate(doorKeyPrefab, pos + new Vector2(.5f, .5f), Quaternion.identity, entityParent);
            goal = Instantiate(goalPrefab, map.GoalPosition + new Vector2(.5f, .5f), Quaternion.identity, entityParent);
        }

        /// <summary>
        ///     プレイヤーがカギを取得したときに呼び出される
        /// </summary>
        /// <param name="key">取得したカギのGameObject</param>
        private void OnPlayerGetDoorKey(Entity.DoorKey.DoorKey key)
        {
            key.Get();
            --doorKeySystem.keyCount.Value;
            if (doorKeySystem.keyCount.Value <= 0)
                OnDoorKeyCountZero();
        }

        /// <summary>
        ///     プレイヤーが死亡したときに呼び出される
        /// </summary>
        private void OnPlayerDeath()
        {
            // 残基があるかどうか判定する
            if (playerLifeSystem.lifeCount.Value <= 1)
            {
                playerLifeSystem.lifeCount.Value = 0;
                OnPlayerLifeZero();
                return;
            }

            // プレイヤーを初期位置まで戻し、操作をリセットする
            player.transform.localPosition = mapSystem.respawnPlayerPosition + new Vector2(.5f, .5f);
            player.SetControl(MoveControl.Stop);
            phaseSystem.SetPhase(Phase.Phase.PlayerDeath);
            --playerLifeSystem.lifeCount.Value;
        }

        /// <summary>
        ///     プレイヤーのライフが0になったときに呼び出される
        /// </summary>
        private void OnPlayerLifeZero()
        {
            phaseSystem.SetPhase(Phase.Phase.GameOver);
        }

        /// <summary>
        ///     マップ上にあるすべてのカギが取得されたときに呼び出される
        /// </summary>
        private void OnDoorKeyCountZero()
        {
            goal.isOpened.Value = true;
        }

        private void OnCollisionToOpenedGoal()
        {
            phaseSystem.SetPhase(Phase.Phase.GameClear);
        }
    }
}