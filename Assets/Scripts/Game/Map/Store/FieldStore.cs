using System;
using UniRx;

namespace Hackman.Game.Map {
    public class FieldStore : IDisposable {

        private readonly CompositeDisposable onDispose = new CompositeDisposable();
        private readonly Subject<UpdateFieldElementEventArgs> onFieldElementUpdated = new Subject<UpdateFieldElementEventArgs>();
        private readonly Subject<MapField> onFieldSet = new Subject<MapField>();

        public MapField Field { get; private set; } = null;
        public IObservable<UpdateFieldElementEventArgs> OnFieldElementUpdated => onFieldElementUpdated;
        public IObservable<MapField> OnFieldSet => onFieldSet;

        public void SetField(MapElement[,] elements) {
            Field = new MapField(elements);
            Field.OnFieldElementUpdated.Subscribe(onFieldElementUpdated).AddTo(onDispose);
            onFieldSet.OnNext(Field);
        }

        public void Dispose() {
            onDispose.Dispose();
        }

    }
}
