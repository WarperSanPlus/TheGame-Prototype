using UnityEngine;

namespace Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 LerpAll(this Vector3 original, Vector3 destination, float duration)
        {
            var copy = original;

            copy.x = Mathf.Lerp(copy.x, destination.x, duration);
            copy.y = Mathf.Lerp(copy.y, destination.y, duration);
            copy.z = Mathf.Lerp(copy.z, destination.z, duration);

            return copy;
        }

        public static Vector3 LerpAngleAll(this Vector3 original, Vector3 destination, float duration)
        {
            var copy = original;

            copy.x = Mathf.LerpAngle(copy.x, destination.x, duration);
            copy.y = Mathf.LerpAngle(copy.y, destination.y, duration);
            copy.z = Mathf.LerpAngle(copy.z, destination.z, duration);

            return copy;
        }
    }
}