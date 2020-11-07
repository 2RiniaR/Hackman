using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Game.System;
using Helper;

namespace Game.Entity.Monster.Brain.Map
{
    public partial class MapGraph
    {
        /// <summary>
        ///     座標とマッピングされていないノードを追加する
        /// </summary>
        public AppendNodeResult AppendNewUnmappedNode()
        {
            var newNode = new DiscreteGraphNode();
            _nodes.Add(newNode.ID, newNode);
            _nodeMap.Add(newNode.ID, MapGraphNode.Unmapped);
            return AppendNodeResult.Succeed(newNode.ID);
        }

        /// <summary>
        ///     座標とマッピングされていないエッジを追加する
        /// </summary>
        /// <returns></returns>
        public AppendEdgeResult AppendNewUnmappedEdge(DiscreteGraphNode.Id startNodeId, DiscreteGraphNode.Id endNodeId, float weight)
        {
            var newEdge = new DiscreteGraphEdge(startNodeId, endNodeId, weight);
            _edges.Add(newEdge.ID, newEdge);
            _edgeMap.Add(newEdge.ID, MapGraphEdge.Unmapped);
            return AppendEdgeResult.Succeed(newEdge.ID);
        }

        /// <summary>
        ///     座標とマッピングされたノードを追加する
        /// </summary>
        public AppendNodeResult AppendNewMappedNode(GridPosition position)
        {
            var newNode = new DiscreteGraphNode();
            _nodes.Add(newNode.ID, newNode);
            _nodeMap.Add(newNode.ID, MapGraphNode.Mapped(position));
            AddPositionMapping(position, DiscreteGraphElement.Node(newNode.ID));
            return AppendNodeResult.Succeed(newNode.ID);
        }

        /// <summary>
        ///     座標とマッピングされたエッジを追加する
        /// </summary>
        /// <returns></returns>
        public AppendEdgeResult AppendNewMappedEdge(in IReadOnlyCollection<DirectPosition> routePositions,
            DiscreteGraphNode.Id startNodeId, DiscreteGraphNode.Id endNodeId, float weight)
        {
            var newEdge = new DiscreteGraphEdge(startNodeId, endNodeId, weight);
            _edges.Add(newEdge.ID, newEdge);
            _edgeMap.Add(newEdge.ID, MapGraphEdge.Unmapped);
            foreach (var routePosition in routePositions)
                AddPositionMapping(routePosition.Position, DiscreteGraphElement.Edge(newEdge.ID));
            return AppendEdgeResult.Succeed(newEdge.ID);
        }

        private void AddPositionMapping(GridPosition position, DiscreteGraphElement element)
        {
            if (!_positionMap.ContainsKey(position))
            {
                var positionElements =
                    new ReadOnlyCollection<DiscreteGraphElement>(new List<DiscreteGraphElement> {element});
                var mapGraphPosition = new MapGraphPosition(positionElements);
                _positionMap.Add(position, mapGraphPosition);
            }
            else
            {
                var positionElements =
                    new ReadOnlyCollection<DiscreteGraphElement>(_positionMap[position].Elements.Concat(new[] {element})
                        .ToList());
                var mapGraphPosition = new MapGraphPosition(positionElements);
                _positionMap[position] = mapGraphPosition;
            }
        }

        private void DeletePositionMapping(GridPosition position, DiscreteGraphElement element)
        {
            if (!_positionMap.ContainsKey(position)) return;
            var positionElements =
                new ReadOnlyCollection<DiscreteGraphElement>(_positionMap[position].Elements.Except(new[] {element})
                    .ToList());
            var mapGraphPosition = new MapGraphPosition(positionElements);
            _positionMap[position] = mapGraphPosition;
        }

        /// <summary>
        ///     ノードを削除する
        /// </summary>
        /// <param name="nodeId"></param>
        public void DeleteNode(DiscreteGraphNode.Id nodeId)
        {
            if (!_nodes.ContainsKey(nodeId)) return;

            // 削除対象のノードに接続されているすべてのエッジを削除する
            foreach (var connectedEdge in _edges.Values.Where(e => e.StartNodeId == nodeId || e.EndNodeId == nodeId))
                DeleteEdge(connectedEdge.ID);

            // 対象のノードがマッピング先に含まれる辞書を更新する
            var mapGraphNode = _nodeMap[nodeId];
            if (mapGraphNode.IsMappingToPosition)
                DeletePositionMapping(mapGraphNode.MappedPosition, DiscreteGraphElement.Node(nodeId));

            // 対象のノードを辞書から削除する
            _nodeMap.Remove(nodeId);
            _nodes.Remove(nodeId);
        }

        /// <summary>
        ///     エッジを削除する
        /// </summary>
        /// <param name="edgeId"></param>
        public void DeleteEdge(DiscreteGraphEdge.Id edgeId)
        {
            if (!_edges.ContainsKey(edgeId)) return;

            // 対象のエッジがマッピング先に含まれる辞書を更新する
            var mapGraphEdge = _edgeMap[edgeId];
            if (mapGraphEdge.IsMappingToPosition)
            {
                foreach (var mappedPosition in mapGraphEdge.MappedPositions)
                    DeletePositionMapping(mappedPosition.Position, DiscreteGraphElement.Edge(edgeId));
            }

            // 対象のエッジを辞書から削除する
            _edgeMap.Remove(edgeId);
            _edges.Remove(edgeId);
        }

        /// <summary>
        ///     ノードの挿入結果を表すオブジェクト
        /// </summary>
        public readonly struct AppendNodeResult
        {
            public readonly DiscreteGraphNode.Id AppendedNodeID;
            public readonly bool IsSucceed;

            public static AppendNodeResult Succeed(DiscreteGraphNode.Id appendedNodeID) => new AppendNodeResult(appendedNodeID, true);
            public static AppendNodeResult Failed => new AppendNodeResult(DiscreteGraphNode.Id.Null, false);

            private AppendNodeResult(DiscreteGraphNode.Id appendedNodeID, bool isSucceed)
            {
                AppendedNodeID = appendedNodeID;
                IsSucceed = isSucceed;
            }
        }

        /// <summary>
        ///     エッジの挿入結果を表すオブジェクト
        /// </summary>
        public readonly struct AppendEdgeResult
        {
            public readonly DiscreteGraphEdge.Id AppendedEdgeID;
            public readonly bool IsSucceed;

            public static AppendEdgeResult Succeed(DiscreteGraphEdge.Id appendedEdgeID) => new AppendEdgeResult(appendedEdgeID, true);
            public static AppendEdgeResult Failed => new AppendEdgeResult(DiscreteGraphEdge.Id.Null, false);

            private AppendEdgeResult(DiscreteGraphEdge.Id appendedEdgeID, bool isSucceed)
            {
                AppendedEdgeID = appendedEdgeID;
                IsSucceed = isSucceed;
            }
        }
    }
}