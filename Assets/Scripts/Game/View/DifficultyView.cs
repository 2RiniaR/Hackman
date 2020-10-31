using UnityEngine;
using TMPro;

namespace Hackman.Game {
    public class DifficultyView : MonoBehaviour {

        [SerializeField]
        private TextMeshProUGUI lifeCountText = null;

        public void SetValue(int count) {
            lifeCountText.text = count.ToString();
        }

    }
}
