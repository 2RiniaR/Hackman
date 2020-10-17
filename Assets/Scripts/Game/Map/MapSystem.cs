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

        private FieldStore fieldStore;
        private TileLoader tileLoader;
        private TileDrawer tileDrawer;

        private void Awake() {
            fieldStore = new FieldStore();
            tileDrawer = new TileDrawer(fieldStore, tilemap, tiles);
            tileLoader = new TileLoader(fieldStore);
            tileLoader.LoadAndSetTile(mapFilePath);
        }

        public MapField GetField() {
            return fieldStore.Field;
        }

    }
}
