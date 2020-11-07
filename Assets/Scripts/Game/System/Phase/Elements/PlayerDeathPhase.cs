using UnityEngine;

namespace Game.System.Phase.Elements
{
    public class PlayerDeathPhase : PhaseElement
    {
        public override void Activate()
        {
            var entities = Object.FindObjectsOfType<Entity.Entity>();
            foreach (var entity in entities) entity.enabled = false;
            var phaseSystem = Object.FindObjectOfType<PhaseSystem>();
            phaseSystem.SetPhase(Phase.GameStart);
        }

        public override void Deactivate()
        {
        }
    }
}