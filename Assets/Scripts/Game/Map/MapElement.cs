namespace Hackman.Game.Map {
    public struct MapElement {
        public readonly Tile Tile;

        public MapElement(Tile tile) {
            Tile = tile;
        }

        public static MapElement None => new MapElement(Tile.None);
    }
}
