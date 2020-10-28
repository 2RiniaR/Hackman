using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hackman.Game.Entity.Monster.Brain
{
    /// <summary>
    ///     グラフ
    /// </summary>
    public readonly struct Graph
    {
        /// <summary>
        ///     含まれるノードの数
        /// </summary>
        public readonly int NodeCount;

        /// <summary>
        ///     含まれるエッジの数
        /// </summary>
        public readonly int EdgeCount;

        /// <summary>
        ///     含まれるノード
        /// </summary>
        public readonly GraphNode[] Nodes;

        /// <summary>
        ///     含まれるエッジ
        /// </summary>
        public readonly GraphEdge[] Edges;

        public Graph(IEnumerable<GraphNode> nodes, IEnumerable<GraphEdge> edges)
        {
            Nodes = nodes as GraphNode[] ?? nodes.ToArray();
            Edges = edges as GraphEdge[] ?? edges.ToArray();
            NodeCount = Nodes.Length;
            EdgeCount = Edges.Length;
        }
    }

    /// <summary>
    ///     グラフのノード(頂点)
    /// </summary>
    public readonly struct GraphNode
    {
        /// <summary>
        ///     識別子
        /// </summary>
        public readonly int ID;

        /// <summary>
        ///     接続されているエッジ
        /// </summary>
        public readonly int[] ConnectedEdgesId;

        /// <summary>
        ///     接続されているエッジの数
        /// </summary>
        public readonly int ConnectedEdgesCount;

        public GraphNode(int id, IEnumerable<int> edgesId)
        {
            ID = id;
            ConnectedEdgesId = edgesId as int[] ?? edgesId.ToArray();
            ConnectedEdgesCount = ConnectedEdgesId.Length;
        }
    }

    /// <summary>
    ///     グラフのエッジ(辺)
    /// </summary>
    public readonly struct GraphEdge
    {
        /// <summary>
        ///     識別子
        /// </summary>
        public readonly int ID;

        /// <summary>
        ///     開始ノードのID
        /// </summary>
        public readonly int StartNodeId;

        /// <summary>
        ///     終点ノードのID
        /// </summary>
        public readonly int EndNodeId;

        /// <summary>
        ///     重み値
        /// </summary>
        public readonly float Weight;

        public GraphEdge(int id, int startNodeId, int endNodeId, float weight)
        {
            ID = id;
            StartNodeId = startNodeId;
            EndNodeId = endNodeId;
            Weight = Mathf.Max(0f, weight);
        }

        public override string ToString()
        {
            return string.Join(", ", "ID: " + ID, "StartNodeID: " + StartNodeId, "EndNodeId: " + EndNodeId,
                "Weight: " + Weight);
        }
    }
}