using Interfaces;
using UnityEngine;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        /// <inheritdoc/>
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent(out ICollidable collider))
                return;

            collider.OnCollision(this);
        }

        #region Virtual

        public virtual void Despawn() => this.gameObject.SetActive(false);

        public virtual void OnReset() {}

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void OnEnable() => this.OnReset();

        #endregion
    }
}