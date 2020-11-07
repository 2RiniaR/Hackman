using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Helper
{
    /// <summary>
    ///     グラフ
    /// </summary>
    public class DiscreteGraph
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
        private readonly Dictionary<DiscreteGraphNode.Id, DiscreteGraphNode> _nodes;

        /// <summary>
        ///     含まれるエッジ
        /// </summary>
        private readonly Dictionary<DiscreteGraphEdge.Id, DiscreteGraphEdge> _edges;

        public DiscreteGraph(IEnumerable<DiscreteGraphNode> nodes, IEnumerable<DiscreteGraphEdge> edges)
        {
            _nodes = nodes.ToDictionary(n => n.ID, n => n);
            _edges = edges.ToDictionary(e => e.ID, e => e);
            NodeCount = _nodes.Count;
            EdgeCount = _edges.Count;
        }

        public IEnumerable<DiscreteGraphNode> AllNodes => _nodes.Values;
        public IEnumerable<DiscreteGraphEdge> AllEdges => _edges.Values;
        public DiscreteGraphNode GetNode(DiscreteGraphNode.Id id) => _nodes.ContainsKey(id) ? _nodes[id] : new DiscreteGraphNode();
        public DiscreteGraphEdge GetEdge(DiscreteGraphEdge.Id id) => _edges.ContainsKey(id) ? _edges[id] : DiscreteGraphEdge.None;
    }

    /// <summary>
    ///     グラフのノード(頂点)
    /// </summary>
    public class DiscreteGraphNode
    {
        public class Id {
            private static int _incremental;
            private readonly int _internalID;
            public static readonly Id Null = new Id();

            public Id() {
                _internalID = _incremental++;
            }

            public override bool Equals(object obj) => obj is Id id && _internalID == id._internalID;
            public override int GetHashCode() => _internalID.GetHashCode();
            public static bool operator ==(Id l, Id r) => !(r is null) && !(l is null) && l._internalID == r._internalID;
            public static bool operator !=(Id l, Id r) => !(r is null) && !(l is null) && l._internalID != r._internalID;
        }

        /// <summary>
        ///     識別子
        /// </summary>
        public readonly Id ID = new Id();
    }

    /// <summary>
    ///     グラフのエッジ(辺)
    /// </summary>
    public class DiscreteGraphEdge
    {
        public class Id {
            private static int _incremental;
            private readonly int _internalID;
            public static readonly Id Null = new Id();

            public Id() {
                _internalID = _incremental++;
            }

            public override bool Equals(object obj) => obj is Id id && _internalID == id._internalID;
            public override int GetHashCode() => _internalID.GetHashCode();
            public static bool operator ==(Id l, Id r) => !(r is null) && !(l is null) && l._internalID == r._internalID;
            public static bool operator !=(Id l, Id r) => !(r is null) && !(l is null) && l._internalID != r._internalID;
        }

        /// <summary>
        ///     識別子
        /// </summary>
        public readonly Id ID = new Id();

        /// <summary>
        ///     開始ノードのID
        /// </summary>
        public readonly DiscreteGraphNode.Id StartNodeId;

        /// <summary>
        ///     終点ノードのID
        /// </summary>
        public readonly DiscreteGraphNode.Id EndNodeId;

        /// <summary>
        ///     重み値
        /// </summary>
        public readonly float Weight;

        public static DiscreteGraphEdge None => new DiscreteGraphEdge(DiscreteGraphNode.Id.Null, DiscreteGraphNode.Id.Null, 0f);

        public DiscreteGraphEdge(DiscreteGraphNode.Id startNodeId, DiscreteGraphNode.Id endNodeId, float weight)
        {
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

    public enum DiscreteGraphElementType
    {
        None,
        Node,
        Edge
    }

    public readonly struct DiscreteGraphElement
    {
        public bool Equals(DiscreteGraphElement other)
        {
            return Type == other.Type && Equals(NodeId, other.NodeId) && Equals(EdgeId, other.EdgeId);
        }

        public override bool Equals(object obj)
        {
            return obj is DiscreteGraphElement other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Type;
                hashCode = (hashCode * 397) ^ (NodeId != null ? NodeId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (EdgeId != null ? EdgeId.GetHashCode() : 0);
                return hashCode;
            }
        }

        public readonly DiscreteGraphElementType Type;
        public readonly DiscreteGraphNode.Id NodeId;
        public readonly DiscreteGraphEdge.Id EdgeId;

        public static DiscreteGraphElement Node(DiscreteGraphNode.Id id)
            => new DiscreteGraphElement(DiscreteGraphElementType.Node, id, DiscreteGraphEdge.Id.Null);

        public static DiscreteGraphElement Edge(DiscreteGraphEdge.Id id)
            => new DiscreteGraphElement(DiscreteGraphElementType.Edge, DiscreteGraphNode.Id.Null, id);

        public static DiscreteGraphElement None
            => new DiscreteGraphElement(DiscreteGraphElementType.None, DiscreteGraphNode.Id.Null,
                DiscreteGraphEdge.Id.Null);

        public static bool operator ==(DiscreteGraphElement l, DiscreteGraphElement r)
        {
            if (l.Type != r.Type) return false;
            switch (l.Type)
            {
                case DiscreteGraphElementType.Node:
                    return l.NodeId == r.NodeId;
                case DiscreteGraphElementType.Edge:
                    return l.EdgeId == r.EdgeId;
            }

            return true;
        }

        public static bool operator !=(DiscreteGraphElement l, DiscreteGraphElement r)
        {
            return !(l == r);
        }

        private DiscreteGraphElement(DiscreteGraphElementType type, DiscreteGraphNode.Id nodeId,
            DiscreteGraphEdge.Id edgeId)
        {
            Type = type;
            NodeId = nodeId;
            EdgeId = edgeId;
        }
    }
}