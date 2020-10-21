using UnityEngine;
using Hackman.Game.Map;
using Hackman.Game.Entity.Player;
using Hackman.Game.Judge.Clear;
using Hackman.Game.Phase;
using UniRx;

namespace Hackman.Game {
    public class Game : MonoBehaviour {

        [SerializeField]
        private Phase.Phase initialPhase = Phase.Phase.Start;

        [SerializeField]
        private MapSystem mapSystem = null;

        [SerializeField]
        private Player player = null;

        public Vector2 PlayerSpawnPosition;

        private ClearJudger clearJudger = null;

        public MapSystem MapSystem => mapSystem;
        public Player Player => player;
        public PhaseActivator PhaseActivator { get; private set; } = null;

        private void Start() {
            PhaseActivator = new PhaseActivator();
            PhaseActivator.SetPhase(initialPhase);
            clearJudger = new ClearJudger(mapSystem);
            clearJudger.OnClear.Subscribe(_ => Debug.Log("Clear!!")).AddTo(this);

            player.OnKilled.Subscribe(_ => PhaseActivator.SetPhase(Phase.Phase.PlayerDeath)).AddTo(this);
        }

    }
}
