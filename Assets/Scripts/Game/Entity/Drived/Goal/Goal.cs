using UniRx;
using UnityEngine;

namespace Hackman.Game.Entity.Goal
{
    public class Goal : Entity
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string openAnimatorVariableName = "isOpened";
        public BoolReactiveProperty isOpened;

        private void Start()
        {
            isOpened.Subscribe(x => _animator.SetBool(openAnimatorVariableName, x)).AddTo(this);
        }
    }
}