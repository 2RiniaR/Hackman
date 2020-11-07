using Game.Entity;
using Game.Entity.Goal;
using Game.Entity.Monster;
using Game.Entity.Player;
using Game.System.DoorKey;
using Game.System.Map;
using Game.System.Phase;
using Game.System.PlayerLife;
using UniRx;
using UnityEngine;

namespace Game.System
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

        private EntityPosition _playerRespawnPosition;
        private Goal _goal;

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
            player.transform.localPosition = map.startPlayerPosition + Entity.Entity.Size / 2;
            mapSystem.SetMap(map);
            _playerRespawnPosition = EntityPosition.FromVector(map.respawnPlayerPosition);
            // foreach (var pos in map.monstersPosition)
            //     Instantiate(monsterPrefab, pos + Entity.Entity.Size / 2, Quaternion.identity, entityParent);
            // foreach (var pos in map.keysPosition)
            //     Instantiate(doorKeyPrefab, pos + Entity.Entity.Size / 2, Quaternion.identity, entityParent);
            // _goal = Instantiate(goalPrefab, map.goalPosition + Entity.Entity.Size / 2, Quaternion.identity,
            //     entityParent);
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
            player.transform.localPosition = _playerRespawnPosition.GetVector() + Entity.Entity.Size / 2;
            player.CurrentControl.Value = new EntityControl(ControlPattern.Stop);
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
            _goal.isOpened.Value = true;
        }

        private void OnCollisionToOpenedGoal()
        {
            phaseSystem.SetPhase(Phase.Phase.GameClear);
        }
    }
}