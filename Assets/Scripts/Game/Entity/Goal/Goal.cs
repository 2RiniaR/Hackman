using UniRx;
using UnityEngine;

namespace Game.Entity.Goal
{
    public class Goal : Entity
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string openAnimatorVariableName = "isOpened";
        public BoolReactiveProperty isOpened;

        protected void Start()
        {
            isOpened.Subscribe(x => animator.SetBool(openAnimatorVariableName, x)).AddTo(this);
        }
    }
}