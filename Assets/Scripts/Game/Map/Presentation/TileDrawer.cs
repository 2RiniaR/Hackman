using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hackman.Game.Map {
    public class TileDrawer {

        private readonly Tilemap tilemap;
        private readonly ReadOnlyCollection<TileBase> tiles;

        public TileDrawer(Tilemap tilemap, IList<TileBase> tiles) {
            this.tilemap = tilemap;
            this.tiles = new ReadOnlyCollection<TileBase>(tiles);
        }

        public void SetTileViews(Tile[,] mapElements) {
            int width = mapElements.GetLength(0);
            int height = mapElements.GetLength(1);
            Vector3Int[] positionArray = Enumerable.Range(0, mapElements.Length)
                .Select(x => new Vector3Int(x % width, x / width, 1))
                .ToArray();
            TileBase[] tileArray = positionArray
                .Select(p => tiles[(int)mapElements[p.x, height - 1 - p.y]])
                .ToArray();
            tilemap.SetTiles(positionArray, tileArray);
        }

    }
}
