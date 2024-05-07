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
            if (target == null)
                return;

            // Lerp position
            var pos = transform.position.LerpAll(target.position, duration);
            transform.position = pos;

            // Lerp angle
            var angle = transform.eulerAngles.LerpAngleAll(target.eulerAngles, duration);
            transform.eulerAngles = angle;
        }

        /// <summary>
        /// Clamps the rotation of the given target
        /// </summary>
        /// <param name="rotation">Current rotation of the target in degrees</param>
        /// <param name="angles">Angles in degrees to try adding to the target</param>
        /// <param name="maxAngles">Maximum angles in degrees on all axis</param>
        /// <param name="clampAxis">Axis to clamp the rotation</param>
        /// <param name="offset">Offset of the rotation</param>
        /// <returns></returns>
        public static Vector3 ClampRotation(
            this Transform target, 
            Vector3 rotation, 
            Vector3 angles, 
            Vector3 maxAngles, 
            Vector3 clampAxis,
            Vector3? offset = null)
        {
            rotation += angles;
            rotation = rotation.ClampAll(maxAngles, clampAxis);

            var copy = rotation;

            if (offset.HasValue)
                rotation += offset.Value;

            target.eulerAngles = rotation;
            //target.rotation = Quaternion.Euler(rotation);
            return copy;
        }
    }
}