using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Game.System;
using Helper;
using UnityEngine;

namespace Game.Entity.Monster.Brain.Map
{
    public partial class MapGraph
    {

        /// <summary>
        ///     マップグラフを離散グラフに変換する
        /// </summary>
        /// <returns></returns>
        private DiscreteGraph Convert()
        {
            return new DiscreteGraph(_nodes.Values, _edges.Values);
        }


        /// <summary>
        ///     任意の座標が、マップグラフ辺の始点からどのくらいの距離にあるかを返す
        /// </summary>
        /// <param name="routePositions"></param>
        /// <param name="position">対象の座標</param>
        /// <param name="eps">誤差</param>
        /// <returns>距離</returns>
        private static float GetDistanceInEdge(in IList<Vector2Int> routePositions, Vector2 position,
            float eps = 1e-6f)
        {
            var distance = 0f;
            for (var i = 0; i < routePositions.Count - 1; i++)
            {
                if (position.IsRange(routePositions[i], routePositions[i + 1], eps))
                {
                    // ルート上の座標が対象の座標と同一だった場合は、その誤差を距離に加算して終了
                    distance += (position - routePositions[i]).magnitude;
                    break;
                }

                // ルート上の座標が対象の座標と同一でないとき、1マス分(=1.0)を距離に加算する
                distance += 1f;
            }

            return distance;
        }

        /// <summary>
        ///     グラフ内に任意の座標に相当するノードを追加する
        /// </summary>
        public AppendNodeResult InsertNodeAt(EntityStatus entityStatus)
        {
            /*
            // 対象座標が、グラフ上のどの位置に所属するのかを取得する
            var graphPosition = GetGraphElement(position, direction);

            switch (graphPosition.Element.Type)
            {
                case MapGraphElementType.Node:
                    // ノードに相当した場合、そのノードを相当するノードとする
                    var nodeId = graphPosition.Element.NodeId;
                    if (!_mapGraphNodeMap.ContainsKey(nodeId)) break;
                    return AppendNodeResult.Succeed(_mapGraphNodeMap[nodeId]);

                case MapGraphElementType.Edge:
                    // エッジに相当した場合、そのエッジにノードを挿入して、挿入したノードを相当するノードとする
                    return InsertNodeToEdge(graphPosition.Element.EdgeId, graphPosition.DistanceFromStart);
            }
            */
            return AppendNodeResult.Failed;
        }
    }
}