using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace Hackman.Game.Entity {
    public class DebugOutput : MonoBehaviour {

        [SerializeField]
        private Entity entity;

        [SerializeField]
        private Text posText;

        [SerializeField]
        private Text directionText;

        [SerializeField]
        private Text speedText;

        [SerializeField]
        private Text controlText;

        private void Start() {
            entity.OnPositionChanged.Select(x => "Position: " + x).Subscribe(x => posText.text = x).AddTo(this);
            entity.OnControlChanged.Select(x => "Control: " + x).Subscribe(x => controlText.text = x).AddTo(this);
            entity.OnDirectionChanged.Select(x => "Direction: " + x).Subscribe(x => directionText.text = x).AddTo(this);
            entity.OnSpeedChanged.Select(x => "Speed: " + x).Subscribe(x => speedText.text = x).AddTo(this);
        }

    }
}
