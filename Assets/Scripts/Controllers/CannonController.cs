using Extensions;
using Interfaces;
using Singletons;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Controller that manages how the cannon behaves
    /// </summary>
    public class CannonController : Controller, Interactable
    {
        #region Rotation

        [Header("Rotation")]
        [SerializeField, Tooltip("Determines how fast the cannon can turn on each axis")]
        private Vector2 turningSpeed;

        [SerializeField, Tooltip("Determines how far the cannon can turn on each axis")]
        private Vector3 maxAnglesSelf;

        [SerializeField, Tooltip("Determines the axis on which the cannon clamps it's rotation")]
        private Vector3 clampAxisSelf;

        private Vector3 defaultRotation;
        private Vector3 angles;
        private Vector2 direction;

        /// <summary>
        /// Updates the rotation of the cannon
        /// </summary>
        /// <param name="direction">Direction of the rotation</param>
        private void UpdateRotation(Vector3 direction) => this.angles = this.transform.ClampRotation(
            this.angles,
            Vector3.Scale(direction, this.turningSpeed),
            this.maxAnglesSelf,
            this.clampAxisSelf,
            this.transform.parent != null ? this.transform.parent.eulerAngles : null
        );

        #endregion

        #region Shoot

        [Header("Cannon Shoot")]
        [SerializeField, Tooltip("Prefab that the cannon will shoot")]
        private GameObject ball;

        [SerializeField, Tooltip("Determines the origin and the direction of the shot")]
        private Transform origin;

        [SerializeField, Min(0), Tooltip("Determines how much force is put on the projectile upon launch")]
        private float strength;

        /// <summary>
        /// Shoots a shot
        /// </summary>
        private void Shoot()
        {
            var projectile = ObjectPools.ObjectPool.GetObject(this.ball);

            if (projectile == null)
                return;

            // Place projectile
            projectile.transform.position = this.origin.position;
            projectile.transform.up = this.origin.forward;

            if (projectile.TryGetComponent(out Rigidbody rb))
            {
                // Apply initial velocity
                rb.velocity = Vector3.zero;
                rb.AddRelativeForce(new Vector3(0, this.strength, 0));
            }
        }

        #endregion

        #region Controller

        /// <inheritdoc/>
        protected override void OnMove(Vector2 direction) => this.direction = direction;

        /// <inheritdoc/>
        protected override void OnFire() => this.Shoot();

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            this.SetIconsVisibility(false);
            this.SetCursorLock(true);

            // Reset the direction
            this.direction = Vector3.zero;

            // Updates the cannon's rotation
            this.defaultRotation = this.transform.eulerAngles;
            this.UpdateRotation(Vector3.zero);
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut()
        {
            this.SetIconsVisibility(true);
            this.SetCursorLock(false);

            // Put back the cannon at it's default rotation
            this.transform.eulerAngles = this.defaultRotation;
        }

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            if (this.direction.magnitude != 0)
                this.UpdateRotation(new Vector3(-this.direction.y, this.direction.x, 0));
        }

        #endregion

        #region Interactable

        /// <inheritdoc/>
        public void OnClick() => ControllerManager.SwitchTo(this);

        #endregion
    }
}