using UnityEngine;

namespace Hackman.Game.Entity
{
    public class MoveSpeedStore
    {
        private float moveSpeed;

        public MoveSpeedStore(float value)
        {
            MoveSpeed = value;
        }

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = Mathf.Max(0f, value);
        }
    }
}