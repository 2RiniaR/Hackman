using UnityEngine;

namespace Game.Entity.Monster
{
    public class Monster : Entity
    {
        public const float ControlCoolTime = 0.5f;
        public float controlCoolTimeLeft;

        protected override void Update()
        {
            base.Update();
            controlCoolTimeLeft = Mathf.Max(0f, controlCoolTimeLeft - Time.deltaTime);
        }
    }
}