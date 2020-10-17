using UnityEngine;
using Hackman.Game.Map;
using Hackman.Game.Entity.Player;
using Hackman.Game.Judge.Clear;
using UniRx;

namespace Hackman.Game {
    public class Game : MonoBehaviour {

        [SerializeField]
        private MapSystem mapSystem = null;

        [SerializeField]
        private Player player = null;

        private ClearJudger clearJudger = null;

        private void Start() {
            clearJudger = new ClearJudger(mapSystem);
            clearJudger.OnClear.Subscribe(_ => Debug.Log("Clear!!")).AddTo(this);
        }

    }
}
