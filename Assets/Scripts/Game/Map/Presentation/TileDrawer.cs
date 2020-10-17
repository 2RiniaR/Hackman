using UnityEngine;
using System;
using UniRx;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hackman.Game.Map {
    public class TileDrawer : IDisposable {

        private const int tilePosZ = 0;

        private readonly CompositeDisposable onDispose = new CompositeDisposable();
        private readonly UnityEngine.Tilemaps.Tilemap tilemap;
        private readonly ReadOnlyCollection<UnityEngine.Tilemaps.TileBase> tiles;

        public TileDrawer(
            FieldStore fieldStore,
            UnityEngine.Tilemaps.Tilemap tilemap,
            IList<UnityEngine.Tilemaps.TileBase> tiles
        ) {
            this.tilemap = tilemap;
            this.tiles = new ReadOnlyCollection<UnityEngine.Tilemaps.TileBase>(tiles);
            fieldStore.OnFieldSet.Subscribe(SetTileViews).AddTo(onDispose);
            fieldStore.OnFieldElementUpdated.Subscribe(UpdateTileView).AddTo(onDispose);
        }

        public void Dispose() {
            onDispose.Dispose();
        }

        public void UpdateTileView(UpdateFieldElementEventArgs args) {
            Vector3Int pos = new Vector3Int(args.UpdatedPosition.x, args.UpdatedPosition.y, tilePosZ);
            tilemap.SetTile(pos, GetTileFromMapElement(args.ElementAfterUpdate));
        }

        public void SetTileViews(MapField field) {
            Vector3Int[] positionArray = Enumerable.Range(0, field.Width * field.Height)
                .Select(x => new Vector3Int(x % field.Width, x / field.Width, tilePosZ))
                .ToArray();
            UnityEngine.Tilemaps.TileBase[] tileArray = positionArray
                .Select(p => {
                    var element = field.GetElement(p.x, p.y);
                    return GetTileFromMapElement(element);
                })
                .ToArray();
            tilemap.SetTiles(positionArray, tileArray);
        }

        private UnityEngine.Tilemaps.TileBase GetTileFromMapElement(MapElement? element) {
            if (element.HasValue) {
                return tiles[(int)element.Value.Tile];
            } else {
                return tiles[(int)MapElement.None.Tile];
            }
        }

    }
}
