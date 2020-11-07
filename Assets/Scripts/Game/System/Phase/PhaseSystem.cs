using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.System.Phase.Elements;
using UniRx;
using UnityEngine;

namespace Game.System.Phase
{
    public class PhaseSystem : MonoBehaviour
    {
        [SerializeField] private Phase initialPhase = Phase.GameStart;

        private readonly ReactiveProperty<Phase> _phase = new ReactiveProperty<Phase>();

        private ReadOnlyDictionary<Phase, PhaseElement> _elements;

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

        private void OnDestroy()
        {
            foreach (var element in _elements.Values)
                element.Dispose();
        }

        public void SetPhase(Phase phase)
        {
            if (_phase.Value == phase) return;
            if (_elements.TryGetValue(_phase.Value, out var previousElement)) previousElement.Deactivate();
            if (_elements.TryGetValue(phase, out var afterElement)) afterElement.Activate();
            _phase.Value = phase;
        }
    }
}