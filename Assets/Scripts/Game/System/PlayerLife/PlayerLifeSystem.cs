﻿using UniRx;
using UnityEngine;

namespace Hackman.Game.PlayerLife
{
    public class PlayerLifeSystem : MonoBehaviour
    {
        public IntReactiveProperty lifeCount;
        [SerializeField] private PlayerLifeView view;

        private void Awake()
        {
            lifeCount.Subscribe(OnLifeCountChanged).AddTo(this);
        }

        private void OnLifeCountChanged(int count)
        {
            view.SetCount(count - 1);
        }
    }
}