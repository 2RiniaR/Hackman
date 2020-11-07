using System;
using Helper;
using UniRx;
using UnityEngine;

namespace Game.System.Map
{
    public readonly struct UpdateFieldElementEventArgs
    {
        public readonly MapField TargetField;
        public readonly GridPosition UpdatedPosition;
        public readonly MapElement ElementBeforeUpdate;
        public readonly MapElement ElementAfterUpdate;

        public UpdateFieldElementEventArgs(MapField target, GridPosition pos, MapElement beforeElement)
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

        private readonly Subject<UpdateFieldElementEventArgs> _onFieldElementUpdated =
            new Subject<UpdateFieldElementEventArgs>();

        public readonly int Height;

        public readonly int Width;

        public MapField(MapElement[,] elements)
        {
            _elements = elements;
            Width = elements.GetLength(0);
            Height = elements.GetLength(1);
        }

        public IObservable<UpdateFieldElementEventArgs> OnFieldElementUpdated => _onFieldElementUpdated;

        public void UpdateElement(GridPosition pos, MapElement element)
        {
            if (!IsFieldRange(pos)) return;
            var beforeElement = _elements[pos.X, pos.Y];
            _elements[pos.X, pos.Y] = element;
            _onFieldElementUpdated.OnNext(
                new UpdateFieldElementEventArgs(this, pos, beforeElement)
            );
        }

        public MapElement[,] GetAllElements()
        {
            return (MapElement[,]) _elements.Clone();
        }

        public MapElement GetElement(GridPosition pos)
        {
            pos = NormalizePosition(pos);
            return _elements[pos.X, pos.Y];
        }

        public bool IsFieldRange(GridPosition pos)
        {
            return pos.X.IsRange(0, Width - 1) && pos.Y.IsRange(0, Height - 1);
        }

        public GridPosition NormalizePosition(GridPosition pos)
        {
            var surplusX = pos.X % Width;
            var surplusY = pos.Y % Height;
            if (surplusX < 0) surplusX = Width + surplusX;
            if (surplusY < 0) surplusY = Height + surplusY;
            return GridPosition.FromVector(new Vector2Int(surplusX, surplusY));
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