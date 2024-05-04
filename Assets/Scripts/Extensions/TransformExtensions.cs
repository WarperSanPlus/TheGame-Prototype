using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Lerps to the target's position and rotation by the given factor
        /// </summary>
        public static void LerpToTarget(this Transform transform, Transform target, float duration)
        {
            if (transform == null || target == null)
                return;

            // Lerp position
            var pos = transform.position.LerpAll(target.position, duration);
            transform.position = pos;

            // Lerp angle
            var angle = transform.eulerAngles.LerpAngleAll(target.eulerAngles, duration);
            transform.eulerAngles = angle;
        }
    }
}