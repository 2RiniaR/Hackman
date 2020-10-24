using UnityEngine;
using System;
using UniRx;

namespace Hackman.Game.Map {

    public struct UpdateFieldElementEventArgs {
        public readonly MapField TargetField;
        public readonly Vector2Int UpdatedPosition;
        public readonly MapElement ElementBeforeUpdate;
        public readonly MapElement ElementAfterUpdate;

        public UpdateFieldElementEventArgs(MapField target, Vector2Int pos, MapElement beforeElement) {
            TargetField = target;
            UpdatedPosition = target.NormalizePosition(pos);
            ElementBeforeUpdate = beforeElement;
            ElementAfterUpdate = target.GetElement(pos);
        }
    }

    public class MapField {

        // 座標系はワールド座標と同じ
        // x: 右方向が正, 左方向が負
        // y: 上方向が正, 下方向が負
        private readonly MapElement[,] elements;
        public readonly int Width;
        public readonly int Height;

        private readonly Subject<UpdateFieldElementEventArgs> onFieldElementUpdated = new Subject<UpdateFieldElementEventArgs>();
        public IObservable<UpdateFieldElementEventArgs> OnFieldElementUpdated => onFieldElementUpdated;

        public MapField(MapElement[,] elements) {
            this.elements = elements;
            Width = elements.GetLength(0);
            Height = elements.GetLength(1);
        }

        public void UpdateElement(int x, int y, MapElement element) {
            UpdateElement(new Vector2Int(x, y), element);
        }

        public void UpdateElement(Vector2Int pos, MapElement element) {
            if (!IsFieldRange(pos)) {
                return;
            }
            var beforeElement = elements[pos.x, pos.y];
            elements[pos.x, pos.y] = element;
            onFieldElementUpdated.OnNext(
                new UpdateFieldElementEventArgs(this, pos, beforeElement)
            );
        }

        public MapElement[,] GetAllElements() {
            return (MapElement[,])elements.Clone();
        }

        public MapElement GetElement(int x, int y) {
            return GetElement(new Vector2Int(x, y));
        }

        public MapElement GetElement(Vector2Int pos) {
            pos = NormalizePosition(pos);
            return elements[pos.x, pos.y];
        }

        public bool IsFieldRange(Vector2Int pos) {
            return pos.x.IsRange(0, Width - 1) && pos.y.IsRange(0, Height - 1);
        }

        public bool IsFieldRange(Vector2 pos) {
            return Mathf.FloorToInt(pos.x).IsRange(0, Width - 1) && Mathf.FloorToInt(pos.y).IsRange(0, Height - 1);
        }

        public Vector2Int NormalizePosition(Vector2Int pos) {
            int surplusX = pos.x % Width;
            int surplusY = pos.y % Height;
            if (surplusX < 0) surplusX = Width + surplusX;
            if (surplusY < 0) surplusY = Height + surplusY;
            return new Vector2Int(surplusX, surplusY);
        }

        public Vector2 NormalizePosition(Vector2 pos) {
            float surplusX = pos.x % Width;
            float surplusY = pos.y % Height;
            if (surplusX < 0) surplusX = Width + surplusX;
            if (surplusY < 0) surplusY = Height + surplusY;
            return new Vector2(surplusX, surplusY);
        }

    }

}
