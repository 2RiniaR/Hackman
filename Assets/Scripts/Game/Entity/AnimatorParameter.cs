using System;
using UnityEngine;

namespace Game.Entity
{
    [Serializable]
    public struct AnimatorParameter
    {
        public Animator animator;
        public string horizontalName;
        public string verticalName;
    }
}