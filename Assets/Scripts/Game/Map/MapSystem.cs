using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;
using UniRx;

namespace Hackman.Game.Map {
    public class MapSystem : MonoBehaviour {
        // W: 28, H: 31

        [SerializeField]
        private Tilemap tilemap = null;

        [SerializeField]
        private List<TileBase> tiles = null;

        [SerializeField]
        private string mapFilePath = "";

        private FieldStore fieldStore = null;
        private TileLoader tileLoader = null;
        private TileDrawer tileDrawer = null;

        public MapField Field => fieldStore.Field;
        public IObservable<UpdateFieldElementEventArgs> OnFieldElementUpdated => fieldStore.OnFieldElementUpdated;
        public IObservable<MapField> OnFieldSet => fieldStore.OnFieldSet;

        private void Awake() {
            fieldStore = new FieldStore();
            tileDrawer = new TileDrawer(fieldStore, tilemap, tiles);
            tileLoader = new TileLoader(fieldStore);
            tileLoader.LoadAndSetTile(mapFilePath);
        }

    }
}
