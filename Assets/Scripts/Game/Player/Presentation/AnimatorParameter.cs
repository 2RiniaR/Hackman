using UnityEngine;
using System;

namespace Hackman.Game.Player {
    [Serializable]
    public struct AnimatorParameter {
        public Animator _animator;
        public string HorizontalName;
        public string VerticalName;
    }
}
