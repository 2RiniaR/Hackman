using UnityEngine;
using Hackman.Game.Map;
using System;
using System.Linq;
using UniRx;

namespace Hackman.Game.Entity.Player {
    public class MonsterCollisionChecker {

        const float collisionDistance = 0.5f;
        private readonly Transform transform;
        private readonly IObserver<Unit> onKilledObserver;

        private bool isCollisionInPreviousFrame = false;

        public MonsterCollisionChecker(IObserver<Unit> onKilledObserver, Transform transform) {
            this.onKilledObserver = onKilledObserver;
            this.transform = transform;
        }

        public void CheckCollision() {
            Monster.Monster[] monsters = GameObject.FindObjectsOfType<Monster.Monster>();
            bool isCollision = false;
            foreach (var monster in monsters) {
                if (IsCollision(monster.transform.localPosition, transform.localPosition, collisionDistance)) {
                    isCollision = true;
                    break;
                }
            }
            UpdateCollisionState(isCollision);
        }

        private void UpdateCollisionState(bool isCollision) {
            if (isCollision && !isCollisionInPreviousFrame) {
                onKilledObserver.OnNext(Unit.Default);
            }
            isCollisionInPreviousFrame = isCollision;
        }

        private static bool IsCollision(Vector2 pos1, Vector2 pos2, float distance) {
            return Mathf.Abs(pos1.x - pos2.x) < distance && Mathf.Abs(pos1.y - pos2.y) < distance;
        }

    }
}
