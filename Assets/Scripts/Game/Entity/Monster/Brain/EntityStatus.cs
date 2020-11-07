using Game.System;

namespace Game.Entity.Monster.Brain
{
    public readonly struct EntityStatus
    {
        public readonly EntityPosition Position;
        public readonly Direction Direction;

        public EntityStatus(EntityPosition position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}