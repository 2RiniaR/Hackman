using UnityEngine;
using System.Linq;
using System;
using Hackman.Game.Map;
using UniRx;

namespace Hackman.Game.Judge.Clear {
    public class ClearJudger : IDisposable {

        private static readonly Tile[] countTargetItem = new Tile[] { Tile.Dot, Tile.PowerCookie };

        private readonly CompositeDisposable onDispose = new CompositeDisposable();
        private readonly Subject<Unit> onClear = new Subject<Unit>();
        private readonly MapSystem mapSystem;
        private readonly IntReactiveProperty currentItemCount = new IntReactiveProperty(0);

        public IObservable<Unit> OnClear => onClear;

        public ClearJudger(MapSystem mapSystem) {
            this.mapSystem = mapSystem;
            currentItemCount.Value = CountMapItems(mapSystem.Field);
            mapSystem.OnFieldSet.Subscribe(OnFieldSet).AddTo(onDispose);
            mapSystem.OnFieldElementUpdated.Subscribe(OnFieldElementChanged).AddTo(onDispose);
            currentItemCount.Subscribe(OnItemCountChanged).AddTo(onDispose);
        }

        public void Dispose() {
            onDispose.Dispose();
        }

        private bool IsCountTarget(MapElement element) {
            return countTargetItem.Contains(element.Tile);
        }

        private int CountMapItems(MapField field) {
            int count = 0;
            foreach (var element in field.GetAllElements()) {
                if (IsCountTarget(element)) count++;
            }
            return count;
        }

        private void OnFieldSet(MapField field) {
            currentItemCount.Value = CountMapItems(field);
        }

        private void OnFieldElementChanged(UpdateFieldElementEventArgs args) {
            if (IsCountTarget(args.ElementBeforeUpdate) && !IsCountTarget(args.ElementAfterUpdate)) {
                currentItemCount.Value--;
            }
        }

        private void OnItemCountChanged(int count) {
            if (count == 0) {
                onClear.OnNext(Unit.Default);
            }
        }

    }
}
