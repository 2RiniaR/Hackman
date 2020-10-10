using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace Hackman.Game.Map {
    public class MapSystem : MonoBehaviour {
        // W: 28, H: 31

        [SerializeField]
        private Tilemap tilemap;

        [SerializeField]
        private List<TileBase> tiles;

        [SerializeField]
        private string mapFilePath;

        private TileStore tileStore;
        private TileLoader tileLoader;
        private TileDrawer tileDrawer;

        private void Awake() {
            tileDrawer = new TileDrawer(tilemap, tiles);
            tileStore = new TileStore(tileDrawer);
            tileLoader = new TileLoader(tileStore);
            tileLoader.LoadAndSetTile(mapFilePath);
        }

    }
}
