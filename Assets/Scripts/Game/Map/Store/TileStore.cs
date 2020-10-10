namespace Hackman.Game.Map {
    public class TileStore {

        private readonly TileDrawer drawer;
        private Tile[,] tiles = new Tile[0, 0];

        public TileStore(TileDrawer drawer) {
            this.drawer = drawer;
        }

        public void SetTiles(Tile[,] tiles) {
            this.tiles = tiles;
            drawer.SetTileViews(this.tiles);
        }

        public Tile GetTile(int x, int y) {
            return tiles[x, y];
        }

    }
}
