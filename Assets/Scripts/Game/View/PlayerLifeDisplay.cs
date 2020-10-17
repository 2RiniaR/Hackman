using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Hackman.Game {
    public class PlayerLifeDisplay : MonoBehaviour {

        [SerializeField]
        private Text lifeCountText = null;

        private void Start() {
            SetLifeCount(3);
        }

        private void SetLifeCount(int count) {
            lifeCountText.text = count.ToString();
        }

    }
}
