using Hackman.Game.Map;
using UnityEngine;

namespace Hackman.Game.Camera
{
    public class FollowEntityCamera : MonoBehaviour
    {
        [SerializeField] private Vector2 displaySize;
        [SerializeField] private Entity.Entity target;
        [SerializeField] private MapSystem mapSystem;

        private void Update()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            var targetPosition = target.transform.position;
            var minPosition = Vector2.zero + displaySize / 2;
            var maxPosition = new Vector2(mapSystem.Field.Width, mapSystem.Field.Height) - displaySize / 2;
            var cameraPosition = new Vector2(
                Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x),
                Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y));
            transform.position = new Vector3(cameraPosition.x, cameraPosition.y, transform.position.z);
        }
    }
}