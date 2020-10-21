using UnityEngine;
using System.Collections;

namespace Hackman.Game.Phase {
    public class PlayerDeathPhase : PhaseElement {

        public override void Activate() {
            var entities = GameObject.FindObjectsOfType<Entity.Entity>();
            foreach (var entity in entities) {
                entity.enabled = false;
            }
            var game = GameObject.FindObjectOfType<Game>();
            game.Player.transform.localPosition = game.PlayerSpawnPosition;
            game.PhaseActivator.SetPhase(Phase.Start);
        }

        public override void Deactivate() {

        }

    }
}
