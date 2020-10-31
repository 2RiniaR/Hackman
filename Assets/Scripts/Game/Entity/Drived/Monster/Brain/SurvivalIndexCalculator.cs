using System.Collections.Generic;
using System.Linq;

namespace Hackman.Game.Entity.Monster.Brain
{
    public static class SurvivalIndexCalculator
    {
        /// <summary>
        ///     マップグラフから、プレイヤーとモンスターの位置が考慮されたグラフを取得する
        /// </summary>
        /// <param name="player">プレイヤー</param>
        /// <param name="monsters">モンスター</param>
        /// <param name="mapGraph">対象のマップグラフ</param>
        /// <returns>グラフ</returns>
        private static GraphExpandResult GetExpandedGraph(EntityStatus player, in IEnumerable<EntityStatus> monsters,
            MapGraph mapGraph)
        {
            var graphConverter = new GraphConverter(mapGraph);
            var playerNode = graphConverter.AppendNode(player.Position, player.Direction);
            var monstersNode = new GraphConverter.AppendNodeResult[monsters.Count()];
            for (var i = 0; i < monsters.Count(); i++)
                monstersNode[i] =
                    graphConverter.AppendNode(monsters.ElementAt(i).Position, monsters.ElementAt(i).Direction);

            return new GraphExpandResult
            {
                ExpandedGraph = graphConverter.ConvertedGraph,
                Player = new EntityGraphPosition
                    {NodeID = playerNode.NodeID, ForwardEdgeID = playerNode.ForwardEdgeID},
                Monsters = monstersNode.Select(m => new EntityGraphPosition
                    {NodeID = m.NodeID, ForwardEdgeID = m.ForwardEdgeID}).ToArray()
            };
        }

        /// <summary>
        /// プレイヤーから各ノードへの最短距離を求める
        /// </summary>
        /// <param name="graph">対象のグラフ</param>
        /// <param name="playerNode">プレイヤーのノードID</param>
        /// <param name="monstersNode">各モンスターのノードID</param>
        /// <returns>プレイヤーから各ノードへの最短距離</returns>
        private static float[] GetNodeDistanceFromPlayer(Graph graph, EntityGraphPosition playerNode,
            in IEnumerable<EntityGraphPosition> monstersNode)
        {
            // グラフから、ダイクストラ法で使用するグラフに変換する
            var dijkstraEdges = new List<Dijkstra.Edge>[graph.NodeCount];
            for (var i = 0; i < dijkstraEdges.Length; i++) dijkstraEdges[i] = new List<Dijkstra.Edge>();

            // 探索の際に無視するエッジを(開始ノードID, 終了ノードID)の順番で格納する
            // プレイヤーの経路距離を計算するとき、任意のノードからモンスターのノードへ向かうエッジを除く
            // (プレイヤーはモンスターと重なるとゲームオーバーになってしまうため)
            var ignoreEdges = monstersNode.SelectMany(m => graph.Nodes[m.NodeID].ConnectedEdgesId);

            foreach (var edge in graph.Edges)
            {
                if (ignoreEdges.Contains(edge.ID)) continue;
                dijkstraEdges[edge.StartNodeId].Add(new Dijkstra.Edge(edge.EndNodeId, edge.Weight));
                dijkstraEdges[edge.EndNodeId].Add(new Dijkstra.Edge(edge.StartNodeId, edge.Weight));
            }

            var dijkstraGraph = new Dijkstra.Graph(dijkstraEdges.Select(s => s.ToArray()).ToArray(), graph.NodeCount);

            // 各ノードのプレイヤーからの最短距離を求める
            Dijkstra.GetDistance(dijkstraGraph, playerNode.NodeID, out var distance, out _);
            return distance;
        }

        /// <summary>
        ///     任意のモンスターから各ノード間の距離を計算する
        /// </summary>
        /// <param name="graph">計算対象のグラフ</param>
        /// <param name="selfNode">自身のノード</param>
        /// <returns>各ノードへの最短距離</returns>
        private static float[] GetNodeDistanceFromMonster(Graph graph, EntityGraphPosition selfNode)
        {
            // モンスターの経路距離を計算するとき、自身のノードにつながっているエッジを有向エッジにしなければいけない
            // (モンスターは進行方向を反転できないため)
            // グラフから、ダイクストラ法で使用するグラフに変換する
            var dijkstraEdges = new List<Dijkstra.Edge>[graph.NodeCount];
            for (var i = 0; i < dijkstraEdges.Length; i++) dijkstraEdges[i] = new List<Dijkstra.Edge>();

            // 自身の進行方向から自身のノードに入ってくるエッジを計算
            var forwardEdge = graph.Edges[selfNode.ForwardEdgeID];
            var forwardNodeID = forwardEdge.StartNodeId != selfNode.NodeID ? forwardEdge.StartNodeId : forwardEdge.EndNodeId;
            var inverseForwardEdge = (forwardNodeID, selfNode.NodeID);

            // 自身から自身の進行方向でない方向に出ていくエッジを計算
            var backwardEdges = graph.Nodes[selfNode.NodeID].ConnectedEdgesId.Except(new[] {selfNode.ForwardEdgeID})
                .Select(edgeID => graph.Edges[edgeID]);
            var backwardNodesID =
                backwardEdges.Select(e => e.StartNodeId != selfNode.NodeID ? e.StartNodeId : e.EndNodeId);
            var inverseBackwardEdges = backwardNodesID.Select(nodeID => (selfNode.NodeID, nodeID));

            // 最短距離の計算の際に無視するエッジをまとめる
            var ignoreEdges = new[] {inverseForwardEdge}.Concat(inverseBackwardEdges);

            foreach (var edge in graph.Edges)
            {
                // エッジの開始ノードが自身のノードであり、かつ自身のノードから出ていく向きのときは追加しない
                if (!ignoreEdges.Contains((edge.StartNodeId, edge.EndNodeId)))
                    dijkstraEdges[edge.StartNodeId].Add(new Dijkstra.Edge(edge.EndNodeId, edge.Weight));

                // エッジの終了ノードが自身のノードであり、かつ自身のノードに入ってくる向きのときは追加しない
                if (!ignoreEdges.Contains((edge.EndNodeId, edge.StartNodeId)))
                    dijkstraEdges[edge.EndNodeId].Add(new Dijkstra.Edge(edge.StartNodeId, edge.Weight));
            }

            // ダイクストラ法のグラフに変換
            var dijkstraGraph = new Dijkstra.Graph(dijkstraEdges.Select(s => s.ToArray()).ToArray(), graph.NodeCount);

            // 各ノードのプレイヤーからの最短距離を求める
            Dijkstra.GetDistance(dijkstraGraph, selfNode.NodeID, out var distance, out _);
            return distance;
        }

        /// <summary>
        ///     全モンスター中から各ノードへの最短距離を返す
        /// </summary>
        /// <param name="graph">対象のグラフ</param>
        /// <param name="monstersNode">各モンスターのノード</param>
        /// <returns>全モンスター中から書くノードへの最短距離</returns>
        private static float[] GetMinNodeDistanceFromMonsters(Graph graph, in IEnumerable<EntityGraphPosition> monstersNode)
        {
            var distance = new float[monstersNode.Count()][];
            for (var i = 0; i < monstersNode.Count(); i++)
                distance[i] = GetNodeDistanceFromMonster(graph, monstersNode.ElementAt(i));

            // 全モンスター中で最短距離を適用する
            var minDistance = new float[graph.NodeCount];
            for (var i = 0; i < graph.NodeCount; i++)
                minDistance[i] = Enumerable.Range(0, 4).Select(monsterIndex => distance[monsterIndex][i]).Min();

            return minDistance;
        }

        /// <summary>
        ///     「プレイヤーの生存指数」を計算して返す
        /// </summary>
        /// <param name="player">プレイヤーの位置情報</param>
        /// <param name="monsters">モンスターの位置情報</param>
        /// <param name="mapGraph">適用するマップのグラフ</param>
        public static float GetSurvivalIndex(EntityStatus player, in IEnumerable<EntityStatus> monsters,
            MapGraph mapGraph)
        {
            // 各エンティティの座標を考慮して拡張したグラフを取得する
            var graphExpandResult = GetExpandedGraph(player, monsters, mapGraph);
            var distanceFromPlayer = GetNodeDistanceFromPlayer(graphExpandResult.ExpandedGraph,
                graphExpandResult.Player, graphExpandResult.Monsters);
            var distanceFromMonster = GetMinNodeDistanceFromMonsters(graphExpandResult.ExpandedGraph,
                graphExpandResult.Monsters);

            //「プレイヤーの生存指数」として、「モンスターからの最短距離」よりも「プレイヤーからの最短距離」のほうが短いノードの数を返す
            var nearPlayerNodeCount = Enumerable.Range(0, graphExpandResult.ExpandedGraph.NodeCount)
                .Count(nodeId => distanceFromPlayer[nodeId] < distanceFromMonster[nodeId]);
            return nearPlayerNodeCount;
        }

        private struct EntityGraphPosition
        {
            public int NodeID;
            public int ForwardEdgeID;
        }

        private struct GraphExpandResult
        {
            public Graph ExpandedGraph;
            public EntityGraphPosition Player;
            public EntityGraphPosition[] Monsters;
        }
    }
}