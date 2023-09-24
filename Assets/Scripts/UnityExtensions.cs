using UnityEngine;

namespace Extensions
{
    public static class UnityExtensions
    {
        public static void LookAtSmooth(this Transform transform, Vector3 target, float speed)
        {
            var targetDirection = target - transform.position;

            var targetRot = Vector3.SignedAngle(Vector3.forward, targetDirection, Vector3.down);
            var targetQuat = Quaternion.AngleAxis(targetRot, Vector3.down);

            transform.transform.rotation = Quaternion.Lerp(transform.rotation, targetQuat, speed * Time.deltaTime);
        }
    }
}