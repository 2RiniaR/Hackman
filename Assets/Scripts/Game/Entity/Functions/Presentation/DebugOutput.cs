using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace Hackman.Game.Player {
    public class DebugOutput : MonoBehaviour {

        [SerializeField]
        private Player player;

        [SerializeField]
        private Text posText;

        [SerializeField]
        private Text directionText;

        [SerializeField]
        private Text speedText;

        [SerializeField]
        private Text controlText;

        private void Start() {
            player.OnPositionChanged.Select(x => "Position: " + x).Subscribe(x => posText.text = x).AddTo(this);
            player.OnControlChanged.Select(x => "Control: " + x).Subscribe(x => controlText.text = x).AddTo(this);
            player.OnDirectionChanged.Select(x => "Direction: " + x).Subscribe(x => directionText.text = x).AddTo(this);
            player.OnSpeedChanged.Select(x => "Speed: " + x).Subscribe(x => speedText.text = x).AddTo(this);
        }

    }
}
