using System;
using UniRx;

namespace Hackman.Game.Map
{
    public class FieldStore : IDisposable
    {
        private readonly CompositeDisposable _onDispose = new CompositeDisposable();

        private readonly Subject<UpdateFieldElementEventArgs> _onFieldElementUpdated =
            new Subject<UpdateFieldElementEventArgs>();

        private readonly BehaviorSubject<MapField> _onFieldSet = new BehaviorSubject<MapField>(new MapField(new MapElement[0, 0]));

        public MapField Field { get; private set; }
        public IObservable<UpdateFieldElementEventArgs> OnFieldElementUpdated => _onFieldElementUpdated;
        public IObservable<MapField> OnFieldSet => _onFieldSet;

        public void Dispose()
        {
            _onDispose.Dispose();
        }

        public void SetField(MapElement[,] elements)
        {
            Field = new MapField(elements);
            Field.OnFieldElementUpdated.Subscribe(_onFieldElementUpdated).AddTo(_onDispose);
            _onFieldSet.OnNext(Field);
        }
    }
}