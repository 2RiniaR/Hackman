using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackman.Game.Entity.Monster.Brain {
    public static class SurvivalIndexCalculator {

        /// <summary>
        ///     マップグラフから、プレイヤーとモンスターの位置が考慮されたグラフを取得する
        /// </summary>
        /// <param name="player">プレイヤー</param>
        /// <param name="monsters">モンスター</param>
        /// <param name="mapGraph">対象のマップグラフ</param>
        /// <param name="playerNodeID">プレイヤーのノードIDが格納される</param>
        /// <param name="monstersNodeID">モンスターのノードIDが格納される</param>
        /// <returns>グラフ</returns>
        private static Graph GetExpandedGraph(EntityStatus player, in IEnumerable<EntityStatus> monsters,
            MapGraph mapGraph, out int playerNodeID, out IEnumerable<int> monstersNodeID)
        {
            var graphConverter = new GraphConverter(mapGraph);
            playerNodeID = graphConverter.AppendNode(player.Position);
            var monstersNodeIDArray = new int[monsters.Count()];
            for (var i = 0; i < monsters.Count(); i++)
            {
                monstersNodeIDArray[i] = graphConverter.AppendNode(monsters.ElementAt(i).Position);
            }

            monstersNodeID = monstersNodeIDArray;
            return graphConverter.ConvertedGraph;
        }

        private static float[] GetNodeDistanceFromPlayer(Graph graph, int playerNodeID, in IEnumerable<int> monstersNodeID)
        {
            // グラフから、ダイクストラ法で使用するグラフに変換する
            var dijkstraEdges = new List<Dijkstra.Edge>[graph.NodeCount];
            for (var i = 0; i < dijkstraEdges.Length; i++) dijkstraEdges[i] = new List<Dijkstra.Edge>();
            foreach (var edge in graph.Edges)
            {
                // プレイヤーの経路距離を計算するとき、モンスターのノードを切断されている扱いにしなければいけない
                // (プレイヤーはモンスターと重なるとゲームオーバーになってしまうため)
                if (monstersNodeID.Contains(edge.StartNodeId) || monstersNodeID.Contains(edge.EndNodeId))
                {
                    continue;
                }
                dijkstraEdges[edge.StartNodeId].Add(new Dijkstra.Edge(edge.EndNodeId, edge.Weight));
                dijkstraEdges[edge.EndNodeId].Add(new Dijkstra.Edge(edge.StartNodeId, edge.Weight));
            }
            var dijkstraGraph = new Dijkstra.Graph(dijkstraEdges.Select(s => s.ToArray()).ToArray(), graph.NodeCount);

            // 各ノードのプレイヤーからの最短距離を求める
            Dijkstra.GetDistance(dijkstraGraph, playerNodeID, out var distance, out _);
            return distance;
        }

        private static float[] GetNodeDistanceFromMonster(Graph graph, int playerNodeID, in IEnumerable<int> monstersNodeID, int selfIndex)
        {
            if (selfIndex < 0 || selfIndex >= monstersNodeID.Count()) throw new ArgumentOutOfRangeException(nameof(selfIndex));

            // モンスターの経路距離を計算するとき、自身のノードにつながっているエッジを有向エッジにしなければいけない
            // (モンスターは進行方向を反転できないため)
            // グラフから、ダイクストラ法で使用するグラフに変換する
            var dijkstraEdges = new List<Dijkstra.Edge>[graph.NodeCount];
            for (var i = 0; i < dijkstraEdges.Length; i++) dijkstraEdges[i] = new List<Dijkstra.Edge>();
            foreach (var edge in graph.Edges)
            {
                dijkstraEdges[edge.StartNodeId].Add(new Dijkstra.Edge(edge.EndNodeId, edge.Weight));
                dijkstraEdges[edge.EndNodeId].Add(new Dijkstra.Edge(edge.StartNodeId, edge.Weight));
            }
            var dijkstraGraph = new Dijkstra.Graph(dijkstraEdges.Select(s => s.ToArray()).ToArray(), graph.NodeCount);

            // 各ノードのプレイヤーからの最短距離を求める
            Dijkstra.GetDistance(dijkstraGraph, monstersNodeID.ElementAt(selfIndex), out var distance, out _);
            return distance;
        }

        private static float[] GetMinNodeDistanceFromMonsters(Graph graph, int playerNodeID,
            in IEnumerable<int> monstersNodeID)
        {
            var distance = new float[monstersNodeID.Count()][];
            for (var i = 0; i < monstersNodeID.Count(); i++)
            {
                distance[i] = GetNodeDistanceFromMonster(graph, playerNodeID, monstersNodeID, i);
            }

            // 全モンスター中で最短距離を適用する
            var minDistance = new float[graph.NodeCount];
            for (var i = 0; i < graph.NodeCount; i++)
            {
                minDistance[i] = Enumerable.Range(0, 4).Select(monsterIndex => distance[monsterIndex][i]).Min();
            }

            return minDistance;
        }

        /// <summary>
        /// 「プレイヤーの生存指数」を計算して返す
        /// </summary>
        /// <param name="player">プレイヤーの位置情報</param>
        /// <param name="monsters">モンスターの位置情報</param>
        /// <param name="mapGraph">適用するマップのグラフ</param>
        public static float GetSurvivalIndex(EntityStatus player, in IEnumerable<EntityStatus> monsters, MapGraph mapGraph) {
            // 各エンティティの座標を考慮して拡張したグラフを取得する
            var graph = GetExpandedGraph(player, monsters, mapGraph, out var playerNodeID, out var monstersNodeID);
            var distanceFromPlayer = GetNodeDistanceFromPlayer(graph, playerNodeID, in monstersNodeID);
            var distanceFromMonster = GetMinNodeDistanceFromMonsters(graph, playerNodeID, in monstersNodeID);

            //「プレイヤーの生存指数」として、「モンスターからの最短距離」よりも「プレイヤーからの最短距離」のほうが短いノードの数を返す
            var nearPlayerNodeCount = Enumerable.Range(0, graph.NodeCount).Count(nodeId => distanceFromPlayer[nodeId] < distanceFromMonster[nodeId]);
            return nearPlayerNodeCount;
        }

    }
}
