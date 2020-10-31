using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Hackman.Game.Entity.Monster.Brain
{
    public class GraphConverter
    {
        /// <summary>
        ///     任意の座標が、マップグラフ辺の始点からどのくらいの距離にあるかを返す
        /// </summary>
        /// <param name="edge">対象のマップグラフ辺</param>
        /// <param name="routePositions"></param>
        /// <param name="position">対象の座標</param>
        /// <returns>距離</returns>
        private static float GetDistanceInEdge(IEnumerable<Vector2Int> routePositions, Vector2 position, float eps = 1e-6f)
        {
            var distance = 0f;
            var routePositionsArray = routePositions as Vector2Int[] ?? routePositions.ToArray();
            for (var i = 0; i < routePositionsArray.Length - 1; i++)
            {
                var previousPosition = routePositionsArray[i];
                var nextPosition = routePositionsArray[i + 1];
                var min = new Vector2Int(Mathf.Min(previousPosition.x, nextPosition.x), Mathf.Min(previousPosition.y, nextPosition.y));
                var max = new Vector2Int(Mathf.Max(previousPosition.x, nextPosition.x), Mathf.Max(previousPosition.y, nextPosition.y));
                if (min.x - eps <= position.x && position.x < max.x + eps && min.y - eps <= position.y && position.y < max.y + eps)
                {
                    // ルート上の座標が対象の座標と同一だった場合は、その誤差を距離に加算して終了
                    distance += Mathf.Max(Mathf.Abs(position.x - previousPosition.x),
                        Mathf.Abs(position.y - previousPosition.y));
                    break;
                }

                // ルート上の座標が対象の座標と同一でないとき、1マス分(=1.0)を距離に加算する
                distance += 1f;
            }

            return distance;
        }

        /// <summary>
        ///     任意の座標が、マップグラフ上のどの要素に相当するかを返す
        /// </summary>
        /// <param name="graph">対象のマップグラフ</param>
        /// <param name="position">対象の座標</param>
        /// <returns>positionがgraph内で相当する要素</returns>
        private static GraphPosition GetGraphPosition(MapGraph graph, Vector2 position, Vector2 direction)
        {
            var containedMapElement = graph.GetElement(position);

            switch (containedMapElement.Type)
            {
                case MapGraphElementType.Node:
                    var forwardEdgeID = -1;
                    var forwardMapElement = graph.GetElement(position + direction);
                    if (forwardMapElement.Type == MapGraphElementType.Edge)
                    {
                        foreach (var edgeID in graph.Nodes[containedMapElement.Id].ConnectedEdgesId)
                        {
                            if (forwardMapElement.Id != edgeID) continue;
                            forwardEdgeID = edgeID;
                            break;
                        }
                    }

                    return new GraphPosition
                    {
                        Type = MapGraphElementType.Node,
                        ElementId = containedMapElement.Id,
                        ForwardEdgeID = forwardEdgeID
                    };

                case MapGraphElementType.Edge:
                    var startNodePosition = graph.Nodes[graph.Edges[containedMapElement.Id].StartNodeId].Position;
                    var endNodePosition = graph.Nodes[graph.Edges[containedMapElement.Id].EndNodeId].Position;
                    var routePositions = new []{startNodePosition}
                        .Concat(graph.Edges[containedMapElement.Id].RoutePositions)
                        .Concat(new []{endNodePosition});

                    var isStartNodeForward = false;
                    var forwardPosition = position + direction;
                    foreach (var routePosition in routePositions)
                    {
                        if (routePosition.x <= forwardPosition.x && forwardPosition.x < routePosition.x &&
                            routePosition.y <= forwardPosition.y && forwardPosition.y < routePosition.y)
                        {
                            isStartNodeForward = true;
                            break;
                        }
                        if (routePosition.x <= position.x && position.x < routePosition.x &&
                            routePosition.y <= position.y && position.y < routePosition.y) break;
                    }

                    return new GraphPosition
                    {
                        Type = MapGraphElementType.Edge,
                        ElementId = containedMapElement.Id,
                        DistanceFromStart = GetDistanceInEdge(routePositions, position),
                        IsStartNodeForward = isStartNodeForward
                    };

                default:
                    return new GraphPosition { Type = MapGraphElementType.None };
            }
        }

        private readonly MapGraph _mapGraph;
        public Graph ConvertedGraph { get; private set; }

        public GraphConverter(MapGraph mapGraph)
        {
            _mapGraph = mapGraph;
            ConvertedGraph = Convert(_mapGraph);
        }

        private static Graph Convert(MapGraph mapGraph)
        {
            var graphNodes = mapGraph.Nodes
                .Select(x => new GraphNode(x.Id, x.ConnectedEdgesId))
                .ToList();
            var graphEdges = mapGraph.Edges
                .Select(x => new GraphEdge(x.Id, x.StartNodeId, x.EndNodeId, x.RoutePositions.Length))
                .ToList();
            var entityGraph = new Graph(graphNodes.ToArray(), graphEdges.ToArray());
            return entityGraph;
        }

        /// <summary>
        ///     グラフの辺の中にノードを挿入する
        /// </summary>
        /// <param name="edgeID">挿入する辺のID</param>
        /// <param name="distanceFromStartNode">ノードを挿入する位置</param>
        /// <param name="isStartNodeForward"></param>
        /// <returns></returns>
        private AppendNodeResult AppendNodeToEdge(int edgeID, float distanceFromStartNode, bool isStartNodeForward)
        {
            var nodes = ConvertedGraph.Nodes.ToList();
            var edges = ConvertedGraph.Edges.ToList();

            // エッジに相当した場合、新たにそのエッジ内にノードを生成して、そのノードを該当するノードとする
            var containedEdgeIndex = edges.FindIndex(e => e.ID == edgeID);
            var containedEdge = edges[containedEdgeIndex];

            // 新しく追加するノードのIDを決定
            var newNodeId = nodes.Count;

            // 新しく追加するエッジのIDを決定
            var newEdgeId = edges.Count;

            // ノードを追加する
            var newNode = new GraphNode(newNodeId, new[] {containedEdge.ID, newEdgeId});
            nodes.Add(newNode);

            // EndNode側 と 新しいノード を繋ぐエッジを追加する
            var newEdgeBetweenStartToPosition = new GraphEdge(newEdgeId, newNodeId, containedEdge.EndNodeId,
                containedEdge.Weight - distanceFromStartNode);
            edges.Add(newEdgeBetweenStartToPosition);

            // 既存のエッジを、StartNode側 と 新しいノード を繋ぐエッジに更新する
            var newEdgeBetweenEndToPosition = new GraphEdge(containedEdge.ID, containedEdge.StartNodeId,
                newNodeId, distanceFromStartNode);
            edges[containedEdgeIndex] = newEdgeBetweenEndToPosition;

            // 進行方向のエッジを指定する
            var forwardEdgeId = isStartNodeForward ? containedEdge.ID : newEdgeId;

            // グラフを更新する
            ConvertedGraph = new Graph(nodes, edges);

            return new AppendNodeResult(newNodeId, forwardEdgeId);
        }

        /// <summary>
        ///     グラフ内に任意の座標に相当するノードを追加する
        /// </summary>
        /// <param name="positions"></param>
        public AppendNodeResult AppendNode(Vector2 position, Vector2 direction)
        {
            // 対象座標が、グラフ上のどの位置に所属するのかを取得する
            var graphPosition = GetGraphPosition(_mapGraph, position, direction);

            switch (graphPosition.Type)
            {
                case MapGraphElementType.Node:
                    // ノードに相当した場合、そのノードを相当するノードとする
                    return new AppendNodeResult(graphPosition.ElementId, graphPosition.ForwardEdgeID);

                case MapGraphElementType.Edge:
                    // エッジに相当した場合、そのエッジにノードを挿入して、挿入したノードを相当するノードとする
                    return AppendNodeToEdge(graphPosition.ElementId, graphPosition.DistanceFromStart, graphPosition.IsStartNodeForward);
            }

            return AppendNodeResult.Failed;
        }

        private struct GraphPosition
        {
            public MapGraphElementType Type;

            public int ElementId;

            /// <summary>
            ///    Type が Node のとき、前方向のエッジID
            /// </summary>
            public int ForwardEdgeID;

            /// <summary>
            ///    Type が Edge のとき、StartNode側が前方向であればtrue、そうでなければfalse
            /// </summary>
            public bool IsStartNodeForward;

            /// <summary>
            ///     Type が Edge のとき、StartNode側からの距離
            /// </summary>
            public float DistanceFromStart;
        }

        public readonly struct AppendNodeResult
        {
            public readonly bool IsSucceed;
            public readonly int NodeID;
            public readonly int ForwardEdgeID;

            public AppendNodeResult(int nodeID, int forwardEdgeID) : this(nodeID, forwardEdgeID, true) { }

            public static AppendNodeResult Failed => new AppendNodeResult(-1, -1, false);

            private AppendNodeResult(int nodeID, int forwardEdgeID, bool isSucceed)
            {
                NodeID = nodeID;
                ForwardEdgeID = forwardEdgeID;
                IsSucceed = isSucceed;
            }
        }
    }
}