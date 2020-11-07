using UnityEngine;

namespace Game.View
{
    public class PlayerLifeViewItem : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string showAnimatorVariableName = "isShow";

        public void SetShow(bool isShow)
        {
            animator.SetBool(showAnimatorVariableName, isShow);
        }
    }
}