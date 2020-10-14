namespace Hackman.Game.Map {
    public class TileStore {

        // 座標系はワールド座標と同じ
        // x: 右方向が正, 左方向が負
        // y: 上方向が正, 下方向が負
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

        public Tile[,] GetTiles() {
            return (Tile[,])tiles.Clone();
        }

    }
}
