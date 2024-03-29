using UnityEngine;
using UniRx;
using System;

namespace Hackman.Game.Entity {
    public class AnimationUpdater : IDisposable {

        private readonly CompositeDisposable onDispose = new CompositeDisposable();
        private readonly AnimatorParameter animatorParameter;
        private readonly MoveStatus move;

        public AnimationUpdater(AnimatorParameter animatorParameter, MoveStatus move) {
            this.animatorParameter = animatorParameter;
            move.OnDirectionChanged.Subscribe(SetDirection).AddTo(onDispose);
        }

        public void Dispose() {
            onDispose.Dispose();
        }

        private void SetDirection(Vector2 direction) {
            animatorParameter._animator.SetFloat(animatorParameter.HorizontalName, direction.x);
            animatorParameter._animator.SetFloat(animatorParameter.VerticalName, direction.y);
        }

    }
}
