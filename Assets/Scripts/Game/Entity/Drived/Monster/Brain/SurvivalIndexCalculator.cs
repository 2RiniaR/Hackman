using System.Collections.Generic;
using System.Linq;

namespace Hackman.Game.Entity.Monster.Brain {
    public static class SurvivalIndexCalculator {

        /// <summary>
        /// ダイクストラ法のメソッドに渡すためのグラフに変換する
        /// </summary>
        /// <param name="graph">変換するグラフ</param>
        /// <returns>変換したグラフ</returns>
        private static Dijkstra.Graph ConvertToDijkstraGraph(this Graph graph)
        {
            var dijkstraEdges = new List<Dijkstra.Edge>[graph.NodeCount];
            for (var i = 0; i < dijkstraEdges.Length; i++) dijkstraEdges[i] = new List<Dijkstra.Edge>();
            foreach (var edge in graph.Edges)
            {
                dijkstraEdges[edge.StartNodeId].Add(new Dijkstra.Edge(edge.EndNodeId, edge.Weight));
                dijkstraEdges[edge.EndNodeId].Add(new Dijkstra.Edge(edge.StartNodeId, edge.Weight));
            }
            return new Dijkstra.Graph(dijkstraEdges.Select(s => s.ToArray()).ToArray());
        }

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

        /// <summary>
        /// 「プレイヤーの生存指数」を計算して返す
        /// </summary>
        /// <param name="player">プレイヤーの位置情報</param>
        /// <param name="monsters">モンスターの位置情報</param>
        /// <param name="mapGraph">適用するマップのグラフ</param>
        public static float GetSurvivalIndex(EntityStatus player, in IEnumerable<EntityStatus> monsters, MapGraph mapGraph) {
            // 各エンティティの座標を考慮して拡張したグラフを取得する
            var graph = GetExpandedGraph(player, monsters, mapGraph, out var playerNodeID, out var monstersNodeID);

            // グラフをダイクストラ法のフォーマットに合うように変換する
            var dijkstraGraph = graph.ConvertToDijkstraGraph();

            // 各ノードのプレイヤーからの最短距離を求める
            Dijkstra.GetDistance(dijkstraGraph, playerNodeID, out var distanceFromPlayer, out _);

            // 各ノードのモンスターからの最短距離を求める
            var distanceFromMonsters = new float[monsters.Count()][];
            for (var i = 0; i < monsters.Count(); i++) {
                Dijkstra.GetDistance(dijkstraGraph, monstersNodeID.ElementAt(i), out distanceFromMonsters[i], out var _);
            }

            // 全モンスター中で最短距離を適用する
            var minDistanceFromMonster = Enumerable.Range(0, graph.NodeCount)
                .Select(nodeId => Enumerable.Range(0, 4).Select(monsterIndex => distanceFromMonsters[monsterIndex][nodeId]).Min())
                .ToArray();

            //「プレイヤーの生存指数」として、「モンスターからの最短距離」よりも「プレイヤーからの最短距離」のほうが短いノードの数を返す
            var nearPlayerNodeCount = Enumerable.Range(0, graph.NodeCount).Count(nodeId => distanceFromPlayer[nodeId] < minDistanceFromMonster[nodeId]);
            return nearPlayerNodeCount;
        }

    }
}
