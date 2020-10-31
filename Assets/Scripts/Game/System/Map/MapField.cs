using System;
using UniRx;
using UnityEngine;

namespace Hackman.Game.Map
{
    public readonly struct UpdateFieldElementEventArgs
    {
        public readonly MapField TargetField;
        public readonly Vector2Int UpdatedPosition;
        public readonly MapElement ElementBeforeUpdate;
        public readonly MapElement ElementAfterUpdate;

        public UpdateFieldElementEventArgs(MapField target, Vector2Int pos, MapElement beforeElement)
        {
            TargetField = target;
            UpdatedPosition = target.NormalizePosition(pos);
            ElementBeforeUpdate = beforeElement;
            ElementAfterUpdate = target.GetElement(pos);
        }
    }

    public class MapField
    {
        // 座標系はワールド座標と同じ
        // x: 右方向が正, 左方向が負
        // y: 上方向が正, 下方向が負
        private readonly MapElement[,] _elements;
        public readonly int Height;

        private readonly Subject<UpdateFieldElementEventArgs> _onFieldElementUpdated =
            new Subject<UpdateFieldElementEventArgs>();

        public readonly int Width;

        public MapField(MapElement[,] elements)
        {
            _elements = elements;
            Width = elements.GetLength(0);
            Height = elements.GetLength(1);
        }

        public IObservable<UpdateFieldElementEventArgs> OnFieldElementUpdated => _onFieldElementUpdated;

        public void UpdateElement(int x, int y, MapElement element)
        {
            UpdateElement(new Vector2Int(x, y), element);
        }

        public void UpdateElement(Vector2Int pos, MapElement element)
        {
            if (!IsFieldRange(pos)) return;
            var beforeElement = _elements[pos.x, pos.y];
            _elements[pos.x, pos.y] = element;
            _onFieldElementUpdated.OnNext(
                new UpdateFieldElementEventArgs(this, pos, beforeElement)
            );
        }

        public MapElement[,] GetAllElements()
        {
            return (MapElement[,]) _elements.Clone();
        }

        public MapElement GetElement(int x, int y)
        {
            return GetElement(new Vector2Int(x, y));
        }

        public MapElement GetElement(Vector2Int pos)
        {
            pos = NormalizePosition(pos);
            return _elements[pos.x, pos.y];
        }

        public bool IsFieldRange(Vector2Int pos)
        {
            return pos.x.IsRange(0, Width - 1) && pos.y.IsRange(0, Height - 1);
        }

        public bool IsFieldRange(Vector2 pos)
        {
            return Mathf.FloorToInt(pos.x).IsRange(0, Width - 1) && Mathf.FloorToInt(pos.y).IsRange(0, Height - 1);
        }

        public Vector2Int NormalizePosition(Vector2Int pos)
        {
            var surplusX = pos.x % Width;
            var surplusY = pos.y % Height;
            if (surplusX < 0) surplusX = Width + surplusX;
            if (surplusY < 0) surplusY = Height + surplusY;
            return new Vector2Int(surplusX, surplusY);
        }

        public Vector2 NormalizePosition(Vector2 pos)
        {
            var surplusX = pos.x % Width;
            var surplusY = pos.y % Height;
            if (surplusX < 0) surplusX = Width + surplusX;
            if (surplusY < 0) surplusY = Height + surplusY;
            return new Vector2(surplusX, surplusY);
        }
    }
}