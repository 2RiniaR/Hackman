using System;
using UniRx;
using UnityEngine;

namespace Hackman.Game.DoorKey
{
    public class DoorKeySystem : MonoBehaviour
    {
        private int maxKeyCount;
        public IntReactiveProperty keyCount;
        [SerializeField] private DoorKeyView view;

        private void Awake()
        {
            keyCount.Subscribe(OnKeyCountChanged).AddTo(this);
            maxKeyCount = keyCount.Value;
        }

        private void OnKeyCountChanged(int count)
        {
            view.SetCount(maxKeyCount - count);
        }
    }
}