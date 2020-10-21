using UnityEngine;
using UnityEngine.UI;

namespace Hackman.Game {
    public class PlayerLifeDisplay : MonoBehaviour {

        [SerializeField]
        private Text lifeCountText = null;

        public void SetLifeCount(int count) {
            lifeCountText.text = count.ToString();
        }

    }
}
