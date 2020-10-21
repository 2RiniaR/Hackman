using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx.Triggers;
using UniRx;

namespace Hackman.Game {
    public class GameStartAnimation : MonoBehaviour {

        [SerializeField]
        private Text displayText = null;

        [SerializeField]
        private Animator animator = null;

        [SerializeField]
        private string animatorTriggerName = "Play";

        [SerializeField]
        private string animatorPlayStateName = "Start";

        private ObservableStateMachineTrigger animatorTrigger = null;

        private void Start() {
            animatorTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
        }

        public void SetText(string text) {
            displayText.text = text;
        }

        public void Play(Action finishCallback) {
            animator.SetTrigger(animatorTriggerName);
            animatorTrigger.OnStateExitAsObservable()
                .Where(state => state.StateInfo.IsName(animatorPlayStateName))
                .First()
                .Subscribe(_ => finishCallback())
                .AddTo(this);
        }

    }
}
