using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hackman.Game.Map
{
    public class TileDrawer : IDisposable
    {
        private const int TilePosZ = 0;

        private readonly CompositeDisposable _onDispose = new CompositeDisposable();
        private readonly Tilemap _tilemap;
        private readonly ReadOnlyCollection<TileBase> _tiles;

        public TileDrawer(
            FieldStore fieldStore,
            Tilemap tilemap,
            IList<TileBase> tiles
        )
        {
            _tilemap = tilemap;
            _tiles = new ReadOnlyCollection<TileBase>(tiles);
            fieldStore.OnFieldSet.Subscribe(SetTileViews).AddTo(_onDispose);
            fieldStore.OnFieldElementUpdated.Subscribe(UpdateTileView).AddTo(_onDispose);
        }

        public void Dispose()
        {
            _onDispose.Dispose();
        }

        private void UpdateTileView(UpdateFieldElementEventArgs args)
        {
            var pos = new Vector3Int(args.UpdatedPosition.x, args.UpdatedPosition.y, TilePosZ);
            _tilemap.SetTile(pos, GetTileFromMapElement(args.ElementAfterUpdate));
        }

        private void SetTileViews(MapField field)
        {
            var positionArray = Enumerable.Range(0, field.Width * field.Height)
                .Select(x => new Vector3Int(x % field.Width, x / field.Width, TilePosZ))
                .ToArray();
            var tileArray = positionArray
                .Select(p =>
                {
                    var element = field.GetElement(p.x, p.y);
                    return GetTileFromMapElement(element);
                })
                .ToArray();
            _tilemap.SetTiles(positionArray, tileArray);
        }

        private TileBase GetTileFromMapElement(MapElement? element)
        {
            return element.HasValue ? _tiles[(int) element.Value.Tile] : _tiles[(int) MapElement.None.Tile];
        }
    }
}