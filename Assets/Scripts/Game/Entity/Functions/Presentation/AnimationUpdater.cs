using System;
using UniRx;
using UnityEngine;

namespace Hackman.Game.Entity
{
    public class AnimationUpdater : IDisposable
    {
        private readonly AnimatorParameter _animatorParameter;
        private readonly MoveStatus _move;

        private readonly CompositeDisposable _onDispose = new CompositeDisposable();

        public AnimationUpdater(AnimatorParameter animatorParameter, MoveStatus move)
        {
            _animatorParameter = animatorParameter;
            move.OnDirectionChanged.Subscribe(SetDirection).AddTo(_onDispose);
        }

        public void Dispose()
        {
            _onDispose.Dispose();
        }

        private void SetDirection(Vector2 direction)
        {
            if (_animatorParameter.animator == null) return;
            _animatorParameter.animator.SetFloat(_animatorParameter.horizontalName, direction.x);
            _animatorParameter.animator.SetFloat(_animatorParameter.verticalName, direction.y);
        }
    }
}