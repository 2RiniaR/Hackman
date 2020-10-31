using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx.Triggers;
using TMPro;
using UniRx;

namespace Hackman.Game {
    public class GameStartAnimation : MonoBehaviour {

        [SerializeField]
        private TextMeshProUGUI displayText;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private string animatorTriggerName = "Play";

        [SerializeField]
        private string animatorPlayStateName = "Play";

        private ObservableStateMachineTrigger _animatorTrigger;

        private void Start() {
            _animatorTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
        }

        public void SetText(string text) {
            displayText.text = text;
        }

        public void Play(Action finishCallback) {
            animator.SetTrigger(animatorTriggerName);
            _animatorTrigger.OnStateExitAsObservable()
                .Where(state => state.StateInfo.IsName(animatorPlayStateName))
                .First()
                .Subscribe(_ => finishCallback())
                .AddTo(this);
        }

    }
}
