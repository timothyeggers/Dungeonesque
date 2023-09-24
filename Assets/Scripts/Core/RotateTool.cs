using UnityEngine;

namespace Dungeonesque.Core
{
    /// <summary>
    ///     RotateTool has an indicator that can be dragged around a pivot.
    /// </summary>
    public class RotateTool : MonoBehaviour
    {
        [Tooltip("Indicator is the point moving within the wheel.")] [SerializeField]
        private Rigidbody2D indicator;

        [Tooltip("How far can the indicator travel from the center, a.k.a. the radius.")] [SerializeField]
        private float maxDistanceFromPivot = 0.70f;

        public float Radius => maxDistanceFromPivot;
        public float DistanceFromPivot => Vector2.Distance(pivot, indicator.transform.position);
        public Vector2 DirectionFromPivot => (Vector2)indicator.transform.position - pivot;

        private Vector2 pivot => transform.position;

        public float IndicatorRadius
        {
            get
            {
                if (indicator.TryGetComponent<CircleCollider2D>(out var collider)) return collider.radius;
                return 0.01f;
            }
        }

        public void Move(Vector2 origin, Vector2 destination)
        {
            var point = destination - origin;

            // normalize point magnitude to the scale of the wheel. 100f is an abritrary number, representing the scale of 
            // screen space to world space.
            point /= 100f;
            point = Vector2.ClampMagnitude(point, maxDistanceFromPivot);
            point += pivot;

            var move = Physics2D.OverlapCircle(point, IndicatorRadius, 1 << 9);

            // kind of weird, but basically we want the indicator to move between colliders if the magnitude is long enough
            // otherwise we want the indicator to slide against the collider.
            // ensure continuous detection is enabled for the indicator rigidbody2d.
            if (move == null) indicator.transform.position = point;
            else indicator.MovePosition(point);
        }

        public Vector3 TranslatePositionToRotation()
        {
            return TranslatePositionToRotation(180f, 180f);
        }

        public Vector3 TranslatePositionToRotation(float horizontalClamp, float verticalClamp)
        {
            var point = DirectionFromPivot * DistanceFromPivot;
            var angle = Vector2.SignedAngle(Vector2.up, DirectionFromPivot.normalized);

            point = Vector2.ClampMagnitude(point, 1);

            var yRot = point.x * horizontalClamp;
            var xRot = -point.y * verticalClamp;
            var zRot = point.y + Mathf.Abs(point.y) * 180f;
            zRot = 90 - angle;

            var targetRot = new Vector3(xRot, yRot, -zRot);

            return targetRot;
        }

/*    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistanceFromPivot);
    }*/
    }
}