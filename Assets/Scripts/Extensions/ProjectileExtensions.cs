using Projectiles;
using UnityEngine;

namespace Extensions
{
    public static class ProjectileExtension
    {
        public static Vector3 GetLaunch(this Projectile projectile, Vector3 target, float height, float? gravity = null)
        {
            // Get base gravity
            gravity ??= Physics.gravity.y;
            
            var displacement = target - projectile.transform.position;

            if (displacement.y > height)
                displacement.y = height;

            var time = Mathf.Sqrt(-2 * height / gravity.Value) + Mathf.Sqrt(2 * (displacement.y - height) / gravity.Value);

            return new Vector3(
                displacement.x / time,
                Mathf.Sqrt (-2 * gravity.Value * height) * -Mathf.Sign(gravity.Value),
                displacement.z / time
            );
        }
    }
}