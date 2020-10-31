using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hackman.Game.Map
{
    public class MapSystem : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;

        [SerializeField] private List<TileBase> tiles;

        [SerializeField] public Vector2 respawnPlayerPosition;

        private readonly FieldStore _fieldStore = new FieldStore();
        private TileDrawer _tileDrawer;

        public MapField Field => _fieldStore.Field;

        private void Awake()
        {
            _tileDrawer = new TileDrawer(_fieldStore, tilemap, tiles);
        }

        public void SetMap(GameMap map)
        {
            _fieldStore.SetField(TileLoader.LoadTile(map.ResourcePath));
            respawnPlayerPosition = map.RespawnPlayerPosition;
        }

        private void OnDestroy()
        {
            _tileDrawer.Dispose();
        }
    }
}