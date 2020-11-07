using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Entity.Player
{
    public class CollisionDetector
    {
        private const float CollisionDistance = 0.5f;

        private readonly Dictionary<Entity, bool> _isCollisionInPreviousFrame = new Dictionary<Entity, bool>();
        private readonly Subject<Entity> _onCollision = new Subject<Entity>();
        private readonly Transform _transform;

        public CollisionDetector(Transform transform)
        {
            _transform = transform;
        }

        public IObservable<Entity> OnCollision => _onCollision;

        public void CheckCollision()
        {
            var entities = Object.FindObjectsOfType<Entity>();
            foreach (var entity in entities)
            {
                var isCollision = IsCollision(entity.transform.localPosition, _transform.localPosition,
                    CollisionDistance);
                UpdateCollisionState(entity, isCollision);
            }
        }

        private void UpdateCollisionState(Entity entity, bool isCollision)
        {
            var isCollisionInPreviousFrame =
                _isCollisionInPreviousFrame.ContainsKey(entity) && _isCollisionInPreviousFrame[entity];
            if (isCollision && !isCollisionInPreviousFrame) _onCollision.OnNext(entity);
            if (!_isCollisionInPreviousFrame.ContainsKey(entity))
                _isCollisionInPreviousFrame.Add(entity, isCollision);
            else
                _isCollisionInPreviousFrame[entity] = isCollision;
        }

        private static bool IsCollision(Vector2 pos1, Vector2 pos2, float distance)
        {
            return Mathf.Abs(pos1.x - pos2.x) < distance && Mathf.Abs(pos1.y - pos2.y) < distance;
        }
    }
}