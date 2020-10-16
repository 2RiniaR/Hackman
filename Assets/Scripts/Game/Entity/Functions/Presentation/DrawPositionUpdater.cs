using UnityEngine;
using UniRx;
using System;

namespace Hackman.Game.Entity {
    public class DrawPositionUpdater : IDisposable {

        private readonly CompositeDisposable onDispose = new CompositeDisposable();
        private readonly Transform transform;
        private readonly PositionStatus position;

        public DrawPositionUpdater(Transform transform, PositionStatus position) {
            this.transform = transform;
            position.OnPositionChanged.Subscribe(SetPosition).AddTo(onDispose);
        }

        public void Dispose() {
            onDispose.Dispose();
        }

        private void SetPosition(Vector2 position) {
            transform.localPosition = MapPositionToWorldPosition(position);
        }

        private static Vector2 MapPositionToWorldPosition(Vector2 mapPosition) {
            Vector2 mapWorldPosition = new Vector2(-14f, -15.5f);
            Vector2 playerSize = new Vector2(1f, 1f);
            return mapPosition + mapWorldPosition + (playerSize / 2f);
        }

    }
}
