using UnityEngine;
using System.Linq;

namespace Hackman.Game.Entity.Monster.Brain {
    public struct MapGraph {
        public readonly int NodeCount;
        public readonly int EdgeCount;
        public readonly MapGraphNode[] Nodes;
        public readonly MapGraphEdge[] Edges;

        public MapGraph(MapGraphNode[] nodes, MapGraphEdge[] edges) {
            Nodes = nodes;
            Edges = edges;
            NodeCount = nodes.Length;
            EdgeCount = edges.Length;
        }
    }

    public struct MapGraphNode {
        public readonly int Id;
        public readonly Vector2Int Position;
        public readonly int[] ConnectedEdgesId;

        public MapGraphNode(int id, Vector2Int position, int[] edgesId) {
            Id = id;
            Position = position;
            ConnectedEdgesId = edgesId;
        }
    }

    public struct MapGraphEdge {
        public readonly int Id;
        public readonly Vector2Int[] RoutePositions;
        public readonly int StartNodeId;
        public readonly int EndNodeId;

        public MapGraphEdge(int id, Vector2Int[] route, int startNodeId, int endNodeId) {
            Id = id;
            RoutePositions = route;
            StartNodeId = startNodeId;
            EndNodeId = endNodeId;
        }
    }
}
