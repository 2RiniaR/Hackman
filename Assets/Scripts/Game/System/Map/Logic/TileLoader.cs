using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Hackman.Game.Map
{
    public static class TileLoader
    {
        private static readonly ReadOnlyDictionary<char, Tile> CharMap = new ReadOnlyDictionary<char, Tile>(
            new Dictionary<char, Tile>
            {
                {'x', Tile.None},
                {'0', Tile.Floor},
                {'1', Tile.Wall}
            }
        );

        public static MapElement[,] LoadTile(string resourcePath)
        {
            var rowText = Resources.Load<TextAsset>(resourcePath);
            var tiles = rowText.text.Trim().Split('\n').Select(line => line.Select(ParseTile).ToArray()).ToList();
            var maxColumn = tiles.Select(row => row.Length).Max();

            var elementsArray = new MapElement[maxColumn, tiles.Count];
            for (var row = 0; row < tiles.Count; row++)
            for (var column = 0; column < tiles[row].Length; column++) // テキストファイル上では最後に来る行が、マップ上では y = 0 となるためy座標を逆にする
                elementsArray[column, tiles.Count - 1 - row] = new MapElement(tiles[row][column]);
            return elementsArray;
        }

        private static Tile ParseTile(char c)
        {
            return !CharMap.TryGetValue(c, out var tile) ? default : tile;
        }
    }
}