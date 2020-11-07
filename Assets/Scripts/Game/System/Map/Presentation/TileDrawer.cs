using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.System.Map.Presentation
{
    public class TileDrawer
    {
        private const int TilePosZ = 0;

        private readonly MapSystem _mapSystem;

        public TileDrawer(MapSystem mapSystem)
        {
            _mapSystem = mapSystem;
            _mapSystem.Field.SkipLatestValueOnSubscribe().Subscribe(SetField).AddTo(_mapSystem);
        }

        private void UpdateTileView(UpdateFieldElementEventArgs args)
        {
            var pos = new Vector3Int(args.UpdatedPosition.X, args.UpdatedPosition.Y, TilePosZ);
            _mapSystem.tilemap.SetTile(pos, GetTileFromMapElement(args.ElementAfterUpdate));
        }

        private void SetField(MapField field)
        {
            field.OnFieldElementUpdated.Subscribe(UpdateTileView).AddTo(_mapSystem);
            var positionArray = Enumerable.Range(0, field.Width * field.Height)
                .Select(x => new Vector3Int(x % field.Width, x / field.Width, TilePosZ))
                .ToArray();
            var tileArray = positionArray
                .Select(p =>
                {
                    var position = new Vector2Int(p.x, p.y);
                    var element = field.GetElement(GridPosition.FromVector(position));
                    return GetTileFromMapElement(element);
                })
                .ToArray();
            _mapSystem.tilemap.SetTiles(positionArray, tileArray);
        }

        private TileBase GetTileFromMapElement(MapElement? element)
        {
            return element.HasValue ? _mapSystem.tiles[(int) element.Value.Tile] : _mapSystem.tiles[(int) MapElement.None.Tile];
        }
    }
}