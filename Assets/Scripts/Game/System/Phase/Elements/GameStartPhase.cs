using Game.View;
using UnityEngine;

namespace Game.System.Phase.Elements
{
    public class GameStartPhase : PhaseElement
    {
        public override void Activate()
        {
            var entities = Object.FindObjectsOfType<Entity.Entity>();
            foreach (var entity in entities) entity.enabled = false;

            var gameStartAnimation = Object.FindObjectOfType<GameStartAnimation>();
            gameStartAnimation.Play(() =>
            {
                var phaseSystem = Object.FindObjectOfType<PhaseSystem>();
                phaseSystem.SetPhase(Phase.Active);
            });
        }

        public override void Deactivate()
        {
        }
    }
}