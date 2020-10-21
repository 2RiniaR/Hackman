using UnityEngine;

namespace Hackman.Game.Phase {
    public class StartPhase : PhaseElement {

        public override void Activate() {
            var entities = GameObject.FindObjectsOfType<Entity.Entity>();
            foreach (var entity in entities) {
                entity.enabled = false;
            }

            var gameStartAnimation = GameObject.FindObjectOfType<GameStartAnimation>();
            gameStartAnimation.Play(() => {
                var game = GameObject.FindObjectOfType<Game>();
                game.PhaseActivator.SetPhase(Phase.Active);
            });
        }

        public override void Deactivate() {
        }

    }
}
