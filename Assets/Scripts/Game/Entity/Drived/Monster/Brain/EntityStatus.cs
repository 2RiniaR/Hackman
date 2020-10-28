using UnityEngine;

namespace Hackman.Game.Entity.Monster.Brain {
    public readonly struct EntityStatus {
        public readonly Vector2 Position;
        public readonly Vector2 Direction;

        public EntityStatus(Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction.normalized;
        }
    }
}
