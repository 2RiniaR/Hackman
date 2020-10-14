using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hackman.Game.Map {
    public class TileLoader {

        private static readonly ReadOnlyDictionary<char, Tile> charMap = new ReadOnlyDictionary<char, Tile>(
            new Dictionary<char, Tile>() {
                { '0', Tile.Floor       },
                { '1', Tile.Dot         },
                { '2', Tile.PowerCookie },
                { '3', Tile.Wall        },
            }
        );

        private readonly TileStore tileStore;

        public TileLoader(TileStore tileStore) {
            this.tileStore = tileStore;
        }

        public void LoadAndSetTile(string filepath) {
            List<Tile[]> tiles = new List<Tile[]>();
            int maxColumn = 0;
            using (var reader = new StreamReader(filepath)) {
                while (reader.Peek() > -1) {
                    var tilerow = reader.ReadLine().Select(ParseTile).ToArray();
                    maxColumn = (maxColumn < tilerow.Length) ? tilerow.Length : maxColumn;
                    tiles.Add(tilerow);
                }
            }

            Tile[,] tilesArray = new Tile[maxColumn, tiles.Count];
            for (int row = 0; row < tiles.Count; row++) {
                for (int column = 0; column < tiles[row].Length; column++) {
                    // テキストファイル上では最後に来る行が、マップ上ではy=0となるため
                    // y座標を逆にする
                    tilesArray[column, tiles.Count - 1 - row] = tiles[row][column];
                }
            }
            tileStore.SetTiles(tilesArray);
        }

        private static Tile ParseTile(char c) {
            if (!charMap.TryGetValue(c, out Tile tile)) {
                return default(Tile);
            }
            return tile;
        }

    }
}
