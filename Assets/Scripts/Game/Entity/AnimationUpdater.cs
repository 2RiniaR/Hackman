using System;
using Game.System;
using UniRx;
using UnityEngine;

namespace Game.Entity
{
    public class AnimationUpdater
    {
        private readonly Entity _entity;

        public AnimationUpdater(Entity entity)
        {
            _entity = entity;
            _entity.CurrentAction.Select(m => m.GetDirection()).Subscribe(SetDirection).AddTo(_entity);
        }

        private void SetDirection(Direction direction)
        {
            var parameter = _entity.animatorParameter;
            if (parameter.animator == null) return;
            var vector = direction.GetVector();
            parameter.animator.SetFloat(parameter.horizontalName, vector.x);
            parameter.animator.SetFloat(parameter.verticalName, vector.y);
        }
    }
}