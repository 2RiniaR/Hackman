using UnityEngine;
using UniRx;

namespace Hackman.Game.Phase {
    public class ActivePhase : PhaseElement {

        public override void Activate() {
            var entities = GameObject.FindObjectsOfType<Entity.Entity>();
            foreach (var entity in entities) {
                entity.enabled = true;
            }

            var game = GameObject.FindObjectOfType<Game>();
            game.Player.OnKilled.First().Subscribe(_ => OnPlayerKilled());
        }

        private void OnPlayerKilled() {
            var game = GameObject.FindObjectOfType<Game>();
            game.PhaseActivator.SetPhase(Phase.PlayerDeath);
        }

        public override void Deactivate() {

        }

    }
}
