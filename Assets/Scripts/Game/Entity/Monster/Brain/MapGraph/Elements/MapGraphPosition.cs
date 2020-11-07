using System.Collections.Generic;
using Helper;

namespace Game.Entity.Monster.Brain.Map
{
    public readonly struct MapGraphPosition
    {
        public readonly IReadOnlyCollection<DiscreteGraphElement> Elements;

        public MapGraphPosition(in IReadOnlyCollection<DiscreteGraphElement> elements)
        {
            Elements = elements;
        }
    }
}