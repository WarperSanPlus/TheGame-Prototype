using Extensions;
using Interfaces;
using Singletons;
using UnityEngine;

namespace Controllers
{
    public class CannonController : Controller, Interactable
    {
        #region Rotation

        private Vector3 defaultRotation;
        private Vector3 angles;
        private Vector2 direction;

        [SerializeField]
        private Vector2 turningSpeed;

        [SerializeField]
        private Vector3 maxAnglesSelf;

        [SerializeField]
        private Vector3 clampAxisSelf;

        private void MoveSelf(Vector3 direction) => this.angles = this.transform.ClampRotation(
            this.angles,
            Vector3.Scale(direction, this.turningSpeed),
            this.maxAnglesSelf,
            this.clampAxisSelf,
            this.transform.parent != null ? this.transform.parent.eulerAngles : null
        );

        #endregion

        #region Shoot

        [Header("Cannon Shoot")]
        public GameObject ball;

        [SerializeField]
        private Transform origin;

        [SerializeField, Min(0)]
        private float strength;

        private void Shoot()
        {
            var projectile = this.GetProjectile();

            if (projectile == null)
                return;

            projectile.transform.position = this.origin.position;
            projectile.transform.up = this.origin.forward;

            if (projectile.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
                rb.AddRelativeForce(new Vector3(0, this.strength, 0));
            }
        }

        private GameObject GetProjectile()
        {
            return this.ball;
        }

        #endregion

        #region Controller

        /// <inheritdoc/>
        protected override void OnStart()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        /// <inheritdoc/>
        protected override void OnMove(Vector2 direction) => this.direction = direction;

        /// <inheritdoc/>
        protected override void OnFire() => this.Shoot();

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            this.defaultRotation = this.transform.eulerAngles;
            this.direction = Vector3.zero;
            this.MoveSelf(Vector3.zero);
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut() => this.transform.eulerAngles = this.defaultRotation;

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            if (this.direction.magnitude != 0)
                this.MoveSelf(new Vector3(-this.direction.y, this.direction.x, 0));
        }

        #endregion

        #region Interactable

        /// <inheritdoc/>
        public void OnClick() => ControllerManager.SwitchTo(this);

        #endregion
    }
}