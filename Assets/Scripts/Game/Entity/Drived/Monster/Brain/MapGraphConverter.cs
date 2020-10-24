using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Hackman.Game.Map;

namespace Hackman.Game.Entity.Monster.Brain {
    public static class MapGraphConverter {

        private static Vector2Int[] adjoinDeltas = new Vector2Int[] {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
        };
        private static int[] nodeWayCounts = new int[] { 0, 1, 3, 4 };
        private static Tile[] movableTiles = new Tile[] { Tile.Dot, Tile.PowerCookie, Tile.Floor };

        /// <summary>
        /// 移動可能な要素ならばtrue、そうでなければfalseを返す
        /// </summary>
        private static bool IsMovable(MapElement element) {
            return movableTiles.Contains(element.Tile);
        }

        private static bool IsNode(MapField map, Vector2Int pos, out Vector2Int[] wayDirections) {
            if (!IsMovable(map.GetElement(pos))) {
                wayDirections = new Vector2Int[0];
                return false;
            }
            wayDirections = adjoinDeltas.Where(d => IsMovable(map.GetElement(pos + d))).ToArray();
            int nextWayCount = wayDirections.Length;
            return nodeWayCounts.Contains(nextWayCount);
        }

        private static bool TryGetNextNode(MapField map, Vector2Int pos, Vector2Int dir, out Vector2Int[] route, out Vector2Int nextNode) {
            Vector2Int nextDirection = dir;
            Vector2Int searchingPos = map.NormalizePosition(pos + nextDirection);
            List<Vector2Int> routeList = new List<Vector2Int>();
            while (true) {
                if (IsNode(map, searchingPos, out var wayDirection)) {
                    nextNode = searchingPos;
                    route = routeList.ToArray();
                    return true;
                } else if (searchingPos == pos || !IsMovable(map.GetElement(searchingPos))) {
                    nextNode = Vector2Int.zero;
                    route = new Vector2Int[0];
                    return false;
                }
                routeList.Add(searchingPos);
                nextDirection = wayDirection.Except(new Vector2Int[] { -nextDirection }).First();
                searchingPos = map.NormalizePosition(searchingPos + nextDirection);
            }
        }

        public static MapGraph GetGraph(MapField map) {
            // 任意の初期ノードを取得する
            Vector2Int initNodePos = new Vector2Int(0, 0);
            for (int i = 0; i < map.Width * map.Height; i++) {
                Vector2Int pos = new Vector2Int(i % map.Width, i / map.Width);
                if (IsNode(map, pos, out var ways)) {
                    initNodePos = pos;
                    break;
                }
            }

            int edgeIndex = 0;
            int nodeIndex = 0;
            Dictionary<int, List<int>> edgeRegisters = new Dictionary<int, List<int>>();
            List<MapGraphEdge> edges = new List<MapGraphEdge>();
            Dictionary<Vector2Int, int> nodePositionIndice = new Dictionary<Vector2Int, int>();

            void CollectGraphElements(Vector2Int nodePosition) {
                foreach (var delta in adjoinDeltas) {
                    // 行き先にノードがなければ、終了
                    // 例) 「deltaの方向に進行出来ない」、「辺のみで元の点に戻ってくる」など
                    if (!TryGetNextNode(map, nodePosition, delta, out Vector2Int[] route, out Vector2Int nextNodePosition)) continue;

                    // 接続された先のノードが登録されていない場合、再帰的に関数を実行する
                    if (!nodePositionIndice.ContainsKey(nextNodePosition)) {
                        int addedNodeIndex = nodeIndex++;
                        nodePositionIndice.Add(nextNodePosition, addedNodeIndex);
                        edgeRegisters.Add(addedNodeIndex, new List<int>());
                        CollectGraphElements(nextNodePosition);
                    }

                    // 辺をリストに登録する
                    if (
                        nodePositionIndice.TryGetValue(nodePosition, out int srcNodeIndex) &&
                        nodePositionIndice.TryGetValue(nextNodePosition, out int destNodeIndex) &&
                        !edgeRegisters[srcNodeIndex].Contains(destNodeIndex)
                    ) {
                        edgeRegisters[srcNodeIndex].Add(destNodeIndex);
                        edgeRegisters[destNodeIndex].Add(srcNodeIndex);
                        edges.Add(new MapGraphEdge(edgeIndex++, route, srcNodeIndex, destNodeIndex));
                    }
                }
            };

            CollectGraphElements(initNodePos);

            List<MapGraphNode> nodes = new List<MapGraphNode>();
            foreach (var node in nodePositionIndice) {
                var connectedEdges = edges.Where(e => e.StartNodeId == node.Value || e.EndNodeId == node.Value).Select(e => e.Id).ToArray();
                nodes.Add(new MapGraphNode(node.Value, node.Key, connectedEdges));
            }

            return new MapGraph(nodes.ToArray(), edges.ToArray());
        }

    }
}
