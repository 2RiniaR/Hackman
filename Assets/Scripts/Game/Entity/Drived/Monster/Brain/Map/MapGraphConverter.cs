using System.Collections.Generic;
using System.Linq;
using Hackman.Game.Map;
using UnityEngine;

namespace Hackman.Game.Entity.Monster.Brain
{
    public class MapGraphConverter
    {
        /// <summary>
        ///     移動先のリスト
        /// </summary>
        private static readonly Vector2Int[] AdjoinDeltas =
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0)
        };

        /// <summary>
        ///     進行先の数が以下の場合、タイルをノードとして認識する
        /// </summary>
        private static readonly int[] NodeWayCounts = {0, 1, 3, 4};

        /// <summary>
        ///     移動可能なタイル
        /// </summary>
        private static readonly Tile[] MovableTiles = {Tile.Dot, Tile.PowerCookie, Tile.Floor};

        private readonly Dictionary<int, List<int>> _edgeRegisters = new Dictionary<int, List<int>>();
        private readonly List<MapGraphEdge> _edges = new List<MapGraphEdge>();
        private readonly MapField _map;
        private readonly Dictionary<Vector2Int, int> _nodePositionIndice = new Dictionary<Vector2Int, int>();
        private int _edgeIndex;
        private int _nodeIndex;

        public MapGraphConverter(MapField map)
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
        private static bool IsNode(MapField map, Vector2Int pos, out Vector2Int[] wayDirections)
        {
            if (!IsMovable(map.GetElement(pos)))
            {
                wayDirections = new Vector2Int[0];
                return false;
            }

            wayDirections = AdjoinDeltas.Where(d => IsMovable(map.GetElement(pos + d))).ToArray();
            var nextWayCount = wayDirections.Length;
            return NodeWayCounts.Contains(nextWayCount);
        }

        /// <summary>
        ///     行先で次にたどり着くノードがあるかどうかを返す
        /// </summary>
        /// <param name="map">対象のマップ</param>
        /// <param name="pos">視点座標</param>
        /// <param name="dir">移動方向</param>
        /// <param name="route">次のノードまでの経路座標が格納される</param>
        /// <param name="nextNode">次のノードの座標</param>
        /// <returns>ノードにたどり着ける場合はtrue、そうでなければfalse</returns>
        private static bool TryGetNextNode(MapField map, Vector2Int pos, Vector2Int dir, out Vector2Int[] route,
            out Vector2Int nextNode)
        {
            var nextDirection = dir;
            var searchingPos = map.NormalizePosition(pos + nextDirection);
            var routeList = new List<Vector2Int>();
            while (true)
            {
                if (IsNode(map, searchingPos, out var wayDirection))
                {
                    nextNode = searchingPos;
                    route = routeList.ToArray();
                    return true;
                }

                if (searchingPos == pos || !IsMovable(map.GetElement(searchingPos)))
                {
                    nextNode = Vector2Int.zero;
                    route = new Vector2Int[0];
                    return false;
                }

                routeList.Add(searchingPos);
                nextDirection = wayDirection.Except(new[] {-nextDirection}).First();
                searchingPos = map.NormalizePosition(searchingPos + nextDirection);
            }
        }

        /// <summary>
        ///     マップをグラフに変換する
        /// </summary>
        /// <param name="map">変換対象のマップ</param>
        /// <returns>変換したグラフ</returns>
        public MapGraph GetGraph()
        {
            Collect();

            return new MapGraph(
                (from node in _nodePositionIndice
                    let connectedEdges =
                        _edges
                            .Where(e => e.StartNodeId == node.Value || e.EndNodeId == node.Value)
                            .Select(e => e.Id)
                            .ToArray()
                    select new MapGraphNode(node.Value, node.Key, connectedEdges)).ToArray(),
                _edges.ToArray());
        }

        private void Collect()
        {
            // 任意の初期ノードを取得する
            var initNodePos = new Vector2Int(0, 0);
            for (var i = 0; i < _map.Width * _map.Height; i++)
            {
                var pos = new Vector2Int(i % _map.Width, i / _map.Width);
                if (!IsNode(_map, pos, out _)) continue;
                initNodePos = pos;
                break;
            }

            Collect(initNodePos);
        }

        private void Collect(Vector2Int nodePosition)
        {
            foreach (var delta in AdjoinDeltas)
            {
                // 行き先にノードがなければ、終了
                // 例) 「deltaの方向に進行出来ない」、「辺のみで元の点に戻ってくる」など
                if (!TryGetNextNode(_map, nodePosition, delta, out var route, out var nextNodePosition)) continue;

                // 接続された先のノードが登録されていない場合、再帰的に関数を実行する
                if (!_nodePositionIndice.ContainsKey(nextNodePosition))
                {
                    var addedNodeIndex = _nodeIndex++;
                    _nodePositionIndice.Add(nextNodePosition, addedNodeIndex);
                    _edgeRegisters.Add(addedNodeIndex, new List<int>());
                    Collect(nextNodePosition);
                }

                // 辺をリストに登録する
                if (!_nodePositionIndice.TryGetValue(nodePosition, out var srcNodeIndex) ||
                    !_nodePositionIndice.TryGetValue(nextNodePosition, out var destNodeIndex) ||
                    _edgeRegisters[srcNodeIndex].Contains(destNodeIndex)) continue;
                _edgeRegisters[srcNodeIndex].Add(destNodeIndex);
                _edgeRegisters[destNodeIndex].Add(srcNodeIndex);
                _edges.Add(new MapGraphEdge(_edgeIndex++, route, srcNodeIndex, destNodeIndex));
            }
        }
    }
}