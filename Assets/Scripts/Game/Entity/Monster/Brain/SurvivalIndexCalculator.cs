using System.Collections.Generic;
using System.Linq;
using Game.Entity.Monster.Brain.Map;
using Game.System;
using Game.System.Map;
using Helper;

namespace Game.Entity.Monster.Brain
{
    /// <summary>
    ///     「生存指数」を計算する
    /// </summary>
    public static class SurvivalIndexCalculator
    {
        /*
        private static Dictionary<DiscreteGraphNode.Id, float> GetNodeDistance(MapGraph mapGraph,
            DiscreteGraphNode.Id startNodeId)
        {
            // グラフから、ダイクストラ法で使用するグラフに変換する
            var graph = mapGraph.DiscreteGraph;
            var graphNodesId = new DiscreteGraphNode.Id[graph.NodeCount];
            var graphNodesIdIndexMap = new Dictionary<DiscreteGraphNode.Id, int>();
            var indexCount = 0;

            var dijkstraEdges = new List<Dijkstra.Edge>[graph.NodeCount];
            for (var i = 0; i < dijkstraEdges.Length; i++)
                dijkstraEdges[i] = new List<Dijkstra.Edge>();

            foreach (var edge in graph.AllEdges)
            {
                // 始端ノードを追加する
                int startNodeIndex;
                if (!graphNodesIdIndexMap.ContainsKey(edge.StartNodeId))
                {
                    graphNodesIdIndexMap.Add(edge.StartNodeId, indexCount);
                    graphNodesId[indexCount] = edge.StartNodeId;
                    startNodeIndex = indexCount;
                    indexCount++;
                }
                else
                {
                    startNodeIndex = graphNodesIdIndexMap[edge.StartNodeId];
                }

                // 終端ノードを追加する
                int endNodeIndex;
                if (!graphNodesIdIndexMap.ContainsKey(edge.EndNodeId))
                {
                    graphNodesIdIndexMap.Add(edge.EndNodeId, indexCount);
                    graphNodesId[indexCount] = edge.EndNodeId;
                    endNodeIndex = indexCount;
                    indexCount++;
                }
                else
                {
                    endNodeIndex = graphNodesIdIndexMap[edge.EndNodeId];
                }

                // 辺を追加する
                dijkstraEdges[startNodeIndex].Add(new Dijkstra.Edge(endNodeIndex, edge.Weight));
            }

            var dijkstraGraph = new Dijkstra.Graph(dijkstraEdges.Select(s => s.ToArray()).ToArray(), graph.NodeCount);

            // 各ノードのプレイヤーからの最短距離を求める
            Dijkstra.GetDistance(dijkstraGraph, graphNodesIdIndexMap[startNodeId], out var distance, out _);
            return distance.Select((x, i) => (x, i)).ToDictionary(x => graphNodesId[x.i], x => x.x);
        }

        /// <summary>
        ///     プレイヤーから各ノードへの最短距離を求める
        /// </summary>
        /// <returns>プレイヤーから各ノードへの最短距離</returns>
        private static Dictionary<GridPosition, float> GetNodeDistanceFromPlayer(in MapGraph mapGraph, EntityStatus player,
            in IEnumerable<EntityStatus> monsters)
        {
            var positionsMap = new Dictionary<GridPosition, List<DiscreteGraphNode.Id>>();
            foreach (var node in mapGraph.NodeMap.Where(n => n.Value.IsMappingToPosition))
            {
                var position = node.Value.MappedPosition;
                if (positionsMap.ContainsKey(position))
                    positionsMap[position].Add(node.Key);
                else
                    positionsMap.Add(position, new List<DiscreteGraphNode.Id> {node.Key});
            }

            // モンスターの位置のノードを既存のエッジに挿入する
            var monstersNodeId = new List<DiscreteGraphNode.Id>();
            foreach (var monster in monsters)
            {
                var monsterNodeAppendResult = mapGraph.InsertNodeAt(monster.Position, monster.Direction);
                var monsterInverseNodeAppendResult = mapGraph.InsertNodeAt(monster.Position, monster.Direction.Inverse);
                if (!monsterNodeAppendResult.IsSucceed) continue;
                monstersNodeId.Add(monsterNodeAppendResult.AppendedNodeID);
            }

            // プレイヤーの位置のノードを追加し、その両端を接続する
            var playerNodeAppendResult = mapGraph.AppendNewNode();
            var playerNodeId = playerNodeAppendResult.AppendedNodeID;

            // モンスターのノードを削除する
            foreach (var monsterNodeId in monstersNodeId)
                mapGraph.DeleteNode(monsterNodeId);

            var nodesDistance = GetNodeDistance(mapGraph, playerNodeId);
            return positionsMap.ToDictionary(
                p => p.Key,
                p => p.Value.Where(id => nodesDistance.ContainsKey(id)).Select(id => nodesDistance[id]).Min()
            );
        }

        /// <summary>
        ///     任意のモンスターから各ノード間の距離を計算する
        /// </summary>
        /// <returns>各ノードへの最短距離</returns>
        private static Dictionary<GridPosition, float> GetNodeDistanceFromMonster(MapGraph mapGraph, EntityStatus monster)
        {
            var positionsMap = new Dictionary<GridPosition, List<DiscreteGraphNode.Id>>();
            foreach (var node in mapGraph.NodeMap.Where(n => n.Value.IsMappingToPosition))
            {
                var position = node.Value.MappedPosition;
                if (positionsMap.ContainsKey(position))
                    positionsMap[position].Add(node.Key);
                else
                    positionsMap.Add(position, new List<DiscreteGraphNode.Id> {node.Key});
            }

            // 自身のノードを追加し、進行方向を接続する
            var selfNode = mapGraph.AppendNewMappedNode(monster.Position);

            var nodesDistance = GetNodeDistance(mapGraph, playerNodeId);
            return positionsMap.ToDictionary(
                p => p.Key,
                p => p.Value.Where(id => nodesDistance.ContainsKey(id)).Select(id => nodesDistance[id]).Min()
            );
        }

        /// <summary>
        ///     全モンスター中から各ノードへの最短距離を返す
        /// </summary>
        /// <returns>全モンスター中から書くノードへの最短距離</returns>
        private static Dictionary<GridPosition, float> GetMinNodeDistanceFromMonsters(in MapGraph mapGraph,
            in IEnumerable<EntityStatus> monsters)
        {
            var distance = new Dictionary<GridPosition, float>();
            foreach (var monster in monsters)
            {
                var distanceFromMonster = GetNodeDistanceFromMonster(MapGraph.Clone(mapGraph), monster);
                foreach (var d in distanceFromMonster)
                {
                    if (!distance.ContainsKey(d.Key) || distance[d.Key] >= d.Value) continue;
                    distance[d.Key] = d.Value;
                }
            }

            return distance;
        }
*/

        /// <summary>
        ///     「プレイヤーの生存指数」を計算して返す
        /// </summary>
        /// <param name="player">プレイヤーの位置情報</param>
        /// <param name="monsters">モンスターの位置情報</param>
        /// <param name="mapField">適用するマップ</param>
        public static float GetSurvivalIndex(MapField mapField, EntityStatus player,
            in IEnumerable<EntityStatus> monsters)
        {
            /*
            var mapGraph = MapGraph.FromMapField(mapField);
            var distanceFromPlayer = GetNodeDistanceFromPlayer(MapGraph.Clone(mapGraph), player, monsters);
            var distanceFromMonster = GetMinNodeDistanceFromMonsters(MapGraph.Clone(mapGraph), monsters);

            //「プレイヤーの生存指数」として、「モンスターからの最短距離」よりも「プレイヤーからの最短距離」のほうが短いノードの数を返す
            var nearPlayerNodes = distanceFromPlayer
                .Where(d => distanceFromMonster.TryGetValue(d.Key, out var md) && d.Value < md);
            var totalNearPlayerNodePoint = nearPlayerNodes.Count();
            return totalNearPlayerNodePoint;
            */
            return 0;
        }
    }
}