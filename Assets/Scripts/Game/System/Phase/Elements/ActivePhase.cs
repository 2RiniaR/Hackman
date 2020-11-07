using UnityEngine;

namespace Game.System.Phase.Elements
{
    public class ActivePhase : PhaseElement
    {
        public override void Activate()
        {
            var entities = Object.FindObjectsOfType<Entity.Entity>();
            foreach (var entity in entities) entity.enabled = true;
        }

        public override void Deactivate()
        {
            var entities = Object.FindObjectsOfType<Entity.Entity>();
            foreach (var entity in entities) entity.enabled = false;
        }
    }
}