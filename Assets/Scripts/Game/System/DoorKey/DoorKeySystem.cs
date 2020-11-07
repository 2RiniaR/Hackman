using Game.View;
using UniRx;
using UnityEngine;

namespace Game.System.DoorKey
{
    public class DoorKeySystem : MonoBehaviour
    {
        public IntReactiveProperty keyCount;
        [SerializeField] private DoorKeyView view;
        private int _maxKeyCount;

        private void Awake()
        {
            keyCount.Subscribe(OnKeyCountChanged).AddTo(this);
            _maxKeyCount = keyCount.Value;
        }

        private void OnKeyCountChanged(int count)
        {
            view.SetCount(_maxKeyCount - count);
        }
    }
}