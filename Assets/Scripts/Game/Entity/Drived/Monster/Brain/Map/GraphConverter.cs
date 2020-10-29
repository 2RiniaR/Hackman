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
        ///     任意の座標が、マップ上のどのタイル座標に相当するかを返す
        /// </summary>
        /// <param name="pos">対象の座標</param>
        /// <returns>相当するタイル座標</returns>
        private static IEnumerable<Vector2Int> GetIntegerPositions(Vector2 pos)
        {
            var ix = IntegerRangeHelper.GetIntegerRange2(pos.x, pos.x + 1f);
            var iy = IntegerRangeHelper.GetIntegerRange2(pos.y, pos.y + 1f);
            return ix.Select(x => iy.Select(y => new Vector2Int(x, y))).SelectMany(x => x).ToArray();
        }

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

        private static MapGraphElement GetMapGraphElement(MapGraph graph, Vector2 position)
        {
            // ここで帰ってくる座標には、最大1種類の辺 と 頂点 が格納されていると仮定する
            var integerPositions = GetIntegerPositions(position);
            var containedMapElement = new MapGraphElement {Type = MapGraphElementType.None, Id = 0};
            foreach (var tilePos in integerPositions)
            {
                var elem = graph.Tilemap[tilePos.x, tilePos.y];
                if (elem.Type == MapGraphElementType.Edge)
                {
                    containedMapElement = elem;
                    break;
                }

                if (elem.Type == MapGraphElementType.Node)
                {
                    containedMapElement = elem;
                }
            }

            return containedMapElement;
        }

        /// <summary>
        ///     任意の座標が、マップグラフ上のどの要素に相当するかを返す
        /// </summary>
        /// <param name="graph">対象のマップグラフ</param>
        /// <param name="position">対象の座標</param>
        /// <returns>positionがgraph内で相当する要素</returns>
        private static GraphPosition GetGraphPosition(MapGraph graph, Vector2 position)
        {
            var containedMapElement = GetMapGraphElement(graph, position);

            switch (containedMapElement.Type)
            {
                case MapGraphElementType.Node:
                    return new GraphPosition
                    {
                        Type = MapGraphElementType.Node,
                        ElementId = containedMapElement.Id,
                        DistanceFromStart = 0f
                    };
                case MapGraphElementType.Edge:
                    var startNodePosition = graph.Nodes[graph.Edges[containedMapElement.Id].StartNodeId].Position;
                    var endNodePosition = graph.Nodes[graph.Edges[containedMapElement.Id].EndNodeId].Position;
                    var routePositions = new []{startNodePosition}
                        .Concat(graph.Edges[containedMapElement.Id].RoutePositions)
                        .Concat(new []{endNodePosition});
                    return new GraphPosition
                    {
                        Type = MapGraphElementType.Edge,
                        ElementId = containedMapElement.Id,
                        DistanceFromStart = GetDistanceInEdge(routePositions, position)
                    };
                default:
                    return new GraphPosition
                    {
                        Type = MapGraphElementType.None,
                        ElementId = 0,
                        DistanceFromStart = 0f
                    };
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

        private int AppendNodeToEdge(int edgeID, float distanceFromStartNode)
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

            // エッジを追加する
            var newEdgeBetweenStartToPosition = new GraphEdge(newEdgeId, newNodeId, containedEdge.EndNodeId,
                containedEdge.Weight - distanceFromStartNode);
            edges.Add(newEdgeBetweenStartToPosition);
            var newEdgeBetweenEndToPosition = new GraphEdge(containedEdge.ID, containedEdge.StartNodeId,
                newNodeId, distanceFromStartNode);
            edges[containedEdgeIndex] = newEdgeBetweenEndToPosition;

            ConvertedGraph = new Graph(nodes, edges);
            return newNodeId;
        }

        /// <summary>
        ///     グラフ内に任意の座標に相当するノードを追加する
        /// </summary>
        /// <param name="positions"></param>
        public int AppendNode(Vector2 position)
        {
            // 対象座標が、グラフ上のどの位置に所属するのかを取得する
            var graphPosition = GetGraphPosition(_mapGraph, position);

            switch (graphPosition.Type)
            {
                case MapGraphElementType.Node:
                    // ノードに相当した場合、そのノードを相当するノードとする
                    return graphPosition.ElementId;

                case MapGraphElementType.Edge:
                    return AppendNodeToEdge(graphPosition.ElementId, graphPosition.DistanceFromStart);
            }

            return -1;
        }

        private struct GraphPosition
        {
            public MapGraphElementType Type;
            public int ElementId;
            public float DistanceFromStart;
        }
    }
}