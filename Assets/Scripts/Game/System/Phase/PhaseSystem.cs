using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;
using UnityEngine;

namespace Hackman.Game.Phase
{
    public class PhaseSystem : MonoBehaviour
    {
        [SerializeField] private Phase initialPhase = Phase.GameStart;

        private ReadOnlyDictionary<Phase, PhaseElement> _elements;

        private readonly ReactiveProperty<Phase> _phase = new ReactiveProperty<Phase>();

        private void Start()
        {
            _elements = new ReadOnlyDictionary<Phase, PhaseElement>(
                new Dictionary<Phase, PhaseElement>
                {
                    {Phase.GameStart, new GameStartPhase()},
                    {Phase.Active, new ActivePhase()},
                    {Phase.PlayerDeath, new PlayerDeathPhase()},
                    {Phase.GameClear, new GameClearPhase()},
                    {Phase.GameOver, new GameOverPhase()}
                }
            );
            SetPhase(initialPhase);
        }

        public void SetPhase(Phase phase)
        {
            if (_phase.Value == phase) return;
            if (_elements.TryGetValue(_phase.Value, out var previousElement)) previousElement.Deactivate();
            if (_elements.TryGetValue(phase, out var afterElement)) afterElement.Activate();
            _phase.Value = phase;
        }

        private void OnDestroy()
        {
            foreach (var element in _elements.Values)
                element.Dispose();
        }
    }
}