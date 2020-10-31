namespace Hackman.Game.Map
{
    public readonly struct MapElement
    {
        public readonly Tile Tile;

        public MapElement(Tile tile)
        {
            Tile = tile;
        }

        public static MapElement None => new MapElement(Tile.None);
    }
}