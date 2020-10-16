using UnityEngine;

namespace Hackman.Game.Entity {
    public struct MoveUpdateResult {

        public readonly Vector2 BeforePosition;
        public readonly Vector2 MoveRequestedVector;
        public readonly MoveControl RequestedMoveControl;

        public readonly bool PositionChanged;
        public readonly Vector2 AfterPosition;
        public readonly Vector2 MoveResultVector;

        MoveUpdateResult(Vector2 beforePosition, Vector2 moveRequestedVector, MoveControl requestedMoveControl, Vector2 afterPosition) {
            BeforePosition = beforePosition;
            MoveRequestedVector = moveRequestedVector;
            RequestedMoveControl = requestedMoveControl;
            AfterPosition = afterPosition;
            PositionChanged = BeforePosition != AfterPosition;
            MoveResultVector = AfterPosition - BeforePosition;
        }

        public static MoveUpdateResult Applied(
            Vector2 beforePosition,
            Vector2 moveRequestedVector,
            MoveControl requestedMoveControl,
            Vector2 afterPosition
        ) {
            return new MoveUpdateResult(beforePosition, moveRequestedVector, requestedMoveControl, afterPosition);
        }

    }
}
