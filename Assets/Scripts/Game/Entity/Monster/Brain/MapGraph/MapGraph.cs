using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Game.System;
using Game.System.Map;
using Helper;
using UnityEngine;

namespace Game.Entity.Monster.Brain.Map
{
    /// <summary>
    ///     マップのタイル情報と離散グラフを関連付けるオブジェクト
    /// </summary>
    public partial class MapGraph
    {
        private readonly Dictionary<DiscreteGraphNode.Id, DiscreteGraphNode> _nodes;
        private readonly Dictionary<DiscreteGraphEdge.Id, DiscreteGraphEdge> _edges;

        private readonly Dictionary<GridPosition, MapGraphPosition> _positionMap;
        private readonly Dictionary<DiscreteGraphEdge.Id, MapGraphEdge> _edgeMap;
        private readonly Dictionary<DiscreteGraphNode.Id, MapGraphNode> _nodeMap;

        public IReadOnlyDictionary<DiscreteGraphNode.Id, DiscreteGraphNode> Nodes => _nodes;
        public IReadOnlyDictionary<DiscreteGraphEdge.Id, DiscreteGraphEdge> Edges => _edges;

        public IReadOnlyDictionary<GridPosition, MapGraphPosition> PositionMap => _positionMap;
        public IReadOnlyDictionary<DiscreteGraphEdge.Id, MapGraphEdge> EdgeMap => _edgeMap;
        public IReadOnlyDictionary<DiscreteGraphNode.Id, MapGraphNode> NodeMap => _nodeMap;

        public static MapGraph FromMapField(MapField field) => MapGraphElementExtractor.GetGraph(field);
        public static MapGraph Clone(in MapGraph mapGraph) => new MapGraph(mapGraph);

        /// <summary>
        ///     変換後のグラフ
        ///     <para>負荷軽減のため、Graphプロパティで要求されたときに計算する</para>
        /// </summary>
        private DiscreteGraph _convertedDiscreteGraph;

        /// <summary>
        ///     現在の離散グラフ
        /// </summary>
        public DiscreteGraph DiscreteGraph => _convertedDiscreteGraph ?? (_convertedDiscreteGraph = Convert());

        private MapGraph(in IReadOnlyDictionary<DiscreteGraphNode.Id, DiscreteGraphNode> nodes,
            in IReadOnlyDictionary<DiscreteGraphEdge.Id, DiscreteGraphEdge> edges,
            in IReadOnlyDictionary<DiscreteGraphNode.Id, MapGraphNode> nodeMap,
            in IReadOnlyDictionary<DiscreteGraphEdge.Id, MapGraphEdge> edgeMap)
        {
            _nodes = nodes.ToDictionary(x => x.Key, x => x.Value);
            _edges = edges.ToDictionary(x => x.Key, x => x.Value);
            _edgeMap = edgeMap.ToDictionary(x => x.Key, x => x.Value);
            _nodeMap = nodeMap.ToDictionary(x => x.Key, x => x.Value);
        }

        private MapGraph(in MapGraph mapGraph)
        {
            _nodes = new Dictionary<DiscreteGraphNode.Id, DiscreteGraphNode>(mapGraph._nodes);
            _edges = new Dictionary<DiscreteGraphEdge.Id, DiscreteGraphEdge>(mapGraph._edges);
            _nodeMap = new Dictionary<DiscreteGraphNode.Id, MapGraphNode>(mapGraph._nodeMap);
            _edgeMap = new Dictionary<DiscreteGraphEdge.Id, MapGraphEdge>(mapGraph._edgeMap);
            _positionMap = new Dictionary<GridPosition, MapGraphPosition>(mapGraph._positionMap);
        }
    }
}