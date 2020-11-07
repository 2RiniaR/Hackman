using System;
using System.Collections.Generic;
using Game.System.Map.Logic;
using Game.System.Map.Presentation;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.System.Map
{
    public class MapSystem : MonoBehaviour
    {
        public Tilemap tilemap;

        public List<TileBase> tiles;

        private TileDrawer _tileDrawer;
        public readonly ReactiveProperty<MapField> Field = new ReactiveProperty<MapField>();

        private void Awake()
        {
            _tileDrawer = new TileDrawer(this);
        }

        public void SetMap(GameMap map)
        {
            Field.Value = new MapField(TileLoader.LoadTile(map.resourcePath));
        }
    }
}