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
    public partial class MapGraph
    {
        /// <summary>
        ///   マップを解析し、グラフとして出力する
        /// </summary>
        private class MapGraphElementExtractor
        {
            /// <summary>
            ///     進行先の数が以下の場合、タイルをノードとして認識する
            /// </summary>
            private static readonly int[] NodeWayCounts = {0, 1, 3, 4};

            /// <summary>
            ///     移動可能なタイル
            /// </summary>
            private static readonly Tile[] MovableTiles = {Tile.Floor};

            /// <summary>
            ///     解析対象のマップ
            /// </summary>
            private readonly MapField _map;

            /// <summary>
            ///     キーにマップ上の座標、値に探索中のノードのIDを格納する辞書
            /// </summary>
            private readonly Dictionary<GridPosition, DiscreteGraphNode.Id> _positionNodeMap = new Dictionary<GridPosition, DiscreteGraphNode.Id>();

            private readonly Dictionary<DiscreteGraphNode.Id, DiscreteGraphNode> _nodes = new Dictionary<DiscreteGraphNode.Id, DiscreteGraphNode>();
            private readonly Dictionary<DiscreteGraphEdge.Id, DiscreteGraphEdge> _edges = new Dictionary<DiscreteGraphEdge.Id, DiscreteGraphEdge>();
            private readonly Dictionary<DiscreteGraphEdge.Id, MapGraphEdge> _edgeMap = new Dictionary<DiscreteGraphEdge.Id, MapGraphEdge>();
            private readonly Dictionary<DiscreteGraphNode.Id, MapGraphNode> _nodeMap = new Dictionary<DiscreteGraphNode.Id, MapGraphNode>();

            private MapGraphElementExtractor(MapField map)
            {
                _map = map;
            }

            /// <summary>
            ///     移動可能な要素ならばtrue、そうでなければfalseを返す
            /// </summary>
            private static bool IsMovable(MapElement element)
            {
                return MovableTiles.Contains(element.Tile);
            }

            /// <summary>
            ///     ノードであるかどうかを返す
            /// </summary>
            /// <param name="map">対象のマップ</param>
            /// <param name="pos">対象の座標</param>
            /// <param name="wayDirections">ノードが接続されている方向が格納される</param>
            /// <returns>座標posがmap内でノードである場合はtrue、そうでなければfalse</returns>
            private static bool IsNode(MapField map, GridPosition pos, out Direction[] wayDirections)
            {
                if (!IsMovable(map.GetElement(pos)))
                {
                    wayDirections = new Direction[0];
                    return false;
                }

                wayDirections = Direction.All.Where(d =>
                {
                    var tilePosition = GridPosition.FromVector(pos.GetVector() + d.GetVector());
                    var mapElement = map.GetElement(tilePosition);
                    return IsMovable(mapElement);
                }).ToArray();
                var nextWayCount = wayDirections.Length;
                return NodeWayCounts.Contains(nextWayCount);
            }

            private readonly struct GetNextNodeResult
            {
                public readonly bool HasExist;
                public readonly DirectPosition[] RoutePositions;
                public readonly GridPosition NextNodePosition;

                private GetNextNodeResult(bool hasExist, in IEnumerable<DirectPosition> routePositions,
                    GridPosition nextNodePosition)
                {
                    HasExist = hasExist;
                    RoutePositions = routePositions as DirectPosition[] ?? routePositions.ToArray();
                    NextNodePosition = nextNodePosition;
                }

                public static GetNextNodeResult
                    Node(in IEnumerable<DirectPosition> routePositions, GridPosition nextNodePosition) =>
                    new GetNextNodeResult(true, routePositions, nextNodePosition);

                public static GetNextNodeResult NotExist => new GetNextNodeResult(false, default, default);
            }

            /// <summary>
            ///     行先で次にたどり着くノードがあるかどうかを返す
            /// </summary>
            /// <param name="map">対象のマップ</param>
            /// <param name="pos">視点座標</param>
            /// <param name="dir">移動方向</param>
            private static GetNextNodeResult TryGetNextNode(MapField map, GridPosition pos, Direction dir)
            {
                var routeList = new List<DirectPosition> {new DirectPosition(pos, dir)};
                var nextDirection = dir;
                var searchingPosition = map.NormalizePosition(pos + nextDirection.GetVector());
                while (true)
                {
                    // 探索先がノードだったら、ノードが存在することを返す
                    if (IsNode(map, searchingPosition, out var wayDirection))
                        break;

                    // 探索開始位置に戻ってきたら、ノードが存在しないことを返す
                    if (searchingPosition == pos || !IsMovable(map.GetElement(searchingPosition)))
                        return GetNextNodeResult.NotExist;

                    nextDirection = wayDirection.Except(new[] {nextDirection.Inverse}).First();
                    routeList.Add(new DirectPosition(searchingPosition, nextDirection));
                    var nextPosition = searchingPosition + nextDirection.GetVector();
                    searchingPosition = map.NormalizePosition(nextPosition);
                }

                routeList.Add(new DirectPosition(searchingPosition, nextDirection));
                return GetNextNodeResult.Node(routeList, searchingPosition);
            }

            /// <summary>
            ///     マップをグラフに変換する
            /// </summary>
            /// <returns>変換したグラフ</returns>
            public static MapGraph GetGraph(MapField map)
            {
                var converter = new MapGraphElementExtractor(map);
                converter.Collect();
                return new MapGraph(converter._nodes, converter._edges, converter._nodeMap, converter._edgeMap);
            }

            /// <summary>
            ///     マップからグラフ要素を取得する
            /// </summary>
            private void Collect()
            {
                // 任意の初期ノードを取得する
                var initNodePos = GridPosition.FromVector(Vector2Int.zero);
                for (var i = 0; i < _map.Width * _map.Height; i++)
                {
                    var pos = GridPosition.FromVector(new Vector2Int(i % _map.Width, i / _map.Width));
                    if (!IsNode(_map, pos, out _)) continue;
                    initNodePos = pos;
                    break;
                }

                // 初期ノードから再帰的にグラフ要素を取得する
                Collect(initNodePos);
            }

            /// <summary>
            ///     マップからグラフ要素を取得する
            /// </summary>
            /// <param name="nodePosition">探索開始位置</param>
            private void Collect(GridPosition nodePosition)
            {
                foreach (var direction in Direction.All)
                {
                    // 行き先にノードがなければ、探索終了
                    // 例) 「deltaの方向に進行出来ない」、「辺のみで元の点に戻ってくる」など
                    var nextNode = TryGetNextNode(_map, nodePosition, direction);
                    if (!nextNode.HasExist) continue;

                    // 接続された先のノードが登録されていない場合、再帰的に関数を実行する
                    if (!_positionNodeMap.ContainsKey(nextNode.NextNodePosition))
                    {
                        // ノードを登録する
                        var discreteGraphNode = new DiscreteGraphNode();
                        var mapGraphNode = MapGraphNode.Mapped(nextNode.NextNodePosition);
                        _nodes.Add(discreteGraphNode.ID, discreteGraphNode);
                        _nodeMap.Add(discreteGraphNode.ID, mapGraphNode);
                        _positionNodeMap.Add(nextNode.NextNodePosition, discreteGraphNode.ID);

                        // 再帰的に探索を実行
                        Collect(nextNode.NextNodePosition);
                    }

                    // 辺をリストに登録する
                    if (!_positionNodeMap.TryGetValue(nodePosition, out var startNodeID) ||
                        !_positionNodeMap.TryGetValue(nextNode.NextNodePosition, out var endNodeID)) continue;

                    var discreteGraphEdge = new DiscreteGraphEdge(startNodeID, endNodeID, 0);
                    var mapGraphEdge = MapGraphEdge.Mapped(nextNode.RoutePositions);
                    _edges.Add(discreteGraphEdge.ID, discreteGraphEdge);
                    _edgeMap.Add(discreteGraphEdge.ID, mapGraphEdge);
                }
            }
        }
    }
}