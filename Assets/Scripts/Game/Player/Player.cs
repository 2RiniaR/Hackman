using UnityEngine;

namespace Hackman.Game.Player {
    public class Player : MonoBehaviour {

        [Header("操作")]
        public string moveUpButtonName;
        public string moveDownButtonName;
        public string moveLeftButtonName;
        public string moveRightButtonName;

        [Header("移動")]
        public float moveSpeed;

    }
}
