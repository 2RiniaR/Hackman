using System;
using System.Collections.Generic;
using System.Linq;

namespace Helper
{
    public static class Dijkstra
    {
        /// <summary>
        ///     グラフの各ノードへの最短距離を返す
        /// </summary>
        public static void GetDistance(Graph graph, int startNodeId, out float[] distance, out int[] previousNodeId)
        {
            distance = Enumerable.Range(0, graph.NodeCount).Select(_ => float.MaxValue).ToArray();
            var queue = new PriorityQueue<NodeDistance>();
            previousNodeId = Enumerable.Range(0, graph.NodeCount).Select(_ => -1).ToArray();
            distance[startNodeId] = 0;
            queue.Push(new NodeDistance {Distance = distance[startNodeId], NodeId = startNodeId});

            while (queue.Count != 0)
            {
                var point = queue.Top;
                queue.Pop();
                if (distance[point.NodeId] < point.Distance) continue;
                foreach (var edge in graph.GetEdgesStartAt(point.NodeId))
                {
                    if (distance[edge.To] <= distance[point.NodeId] + edge.Weight) continue;
                    distance[edge.To] = distance[point.NodeId] + edge.Weight;
                    previousNodeId[edge.To] = point.NodeId;
                    queue.Push(new NodeDistance {Distance = distance[edge.To], NodeId = edge.To});
                }
            }
        }

        private struct NodeDistance : IComparable<NodeDistance>
        {
            public float Distance;
            public int NodeId;

            public int CompareTo(NodeDistance obj)
            {
                if (Distance < obj.Distance) return -1;
                return Math.Abs(Distance - obj.Distance) < float.Epsilon ? 0 : 1;
            }
        }

        public readonly struct Graph
        {
            private readonly Edge[][] _edges;
            public readonly int NodeCount;

            public IEnumerable<Edge> GetEdgesStartAt(int id)
            {
                return _edges[id];
            }

            public Graph(Edge[][] edges) : this(edges, edges.SelectMany(e => e).Select(e => e.To).Max() + 1)
            {
            }

            public Graph(Edge[][] edges, int nodeCount)
            {
                _edges = edges;
                NodeCount = nodeCount;
            }
        }

        public readonly struct Edge
        {
            public readonly int To;
            public readonly float Weight;

            public Edge(int to, float weight)
            {
                To = to < 0 ? 0 : to;
                Weight = weight < 0f ? 0f : weight;
            }

            public override string ToString()
            {
                return string.Join(", ", "To: " + To, "Weight: " + Weight);
            }
        }
    }
}