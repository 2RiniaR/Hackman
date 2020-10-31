using UnityEngine;

namespace Hackman.Game {
    public class DoorKeyViewItem : MonoBehaviour {

        [SerializeField]
        private Animator animator;

        [SerializeField] private string showAnimatorVariableName = "isShow";

        public void SetShow(bool isShow)
        {
            animator.SetBool(showAnimatorVariableName, isShow);
        }

    }
}
