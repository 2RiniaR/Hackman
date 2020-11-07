using Game.System.Map;
using UnityEngine;

namespace Game.System.Camera
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
            var maxPosition = mapSystem.Field.HasValue ? new Vector2(mapSystem.Field.Value.Width, mapSystem.Field.Value.Height) - displaySize / 2 : minPosition;
            var cameraPosition = new Vector2(
                Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x),
                Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y));
            var transform1 = transform;
            transform1.position = new Vector3(cameraPosition.x, cameraPosition.y, transform1.position.z);
        }
    }
}