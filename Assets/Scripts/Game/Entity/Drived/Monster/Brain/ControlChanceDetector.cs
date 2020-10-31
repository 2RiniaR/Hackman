using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hackman.Game.Entity.Monster.Brain
{
    public class ControlChanceDetector
    {
        public readonly struct ControlChance
        {
            public readonly int MonsterID;
            public readonly Vector2[] DirectionChoices;

            public ControlChance(int monsterID, IEnumerable<Vector2> directionChoices)
            {
                MonsterID = monsterID;
                DirectionChoices = directionChoices as Vector2[] ?? directionChoices.ToArray();
            }
        }

        public static IEnumerable<ControlChance> Detect(MapGraph mapGraph, in IEnumerable<EntityStatus> monsters)
        {
            var controlChances = new List<ControlChance>();
            foreach (var (monster, index) in monsters.Select((x, i) => (x, i)))
            {
                var mapElement = mapGraph.GetElement(monster.Position);
                if (mapElement.Type != MapGraphElementType.Node) continue;
                var node = mapGraph.Nodes[mapElement.Id];
                var edges = node.ConnectedEdgesId.Select(edgeID => mapGraph.Edges[edgeID]);
                var directions = edges.Select(edge =>
                {
                    var vectorNodeToEdgeStart = edge.RoutePositions[0] - node.Position;
                    var vectorNodeToEdgeEnd = edge.RoutePositions[edge.RoutePositions.Length - 1] - node.Position;
                    return vectorNodeToEdgeStart.magnitude <= vectorNodeToEdgeEnd.magnitude
                        ? vectorNodeToEdgeStart
                        : vectorNodeToEdgeEnd;
                });
                controlChances.Add(
                    new ControlChance(index, directions.Select(x => (Vector2)x).Except(new [] {monster.Direction}))
                    );
            }

            return controlChances;
        }
    }
}