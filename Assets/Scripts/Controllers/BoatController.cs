using Extensions;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Controller that manages how the boat behaves
    /// </summary>
    public class BoatController : Controller
    {
        [SerializeField]
        private GameObject[] icons;

        #region Move

        [Header("Move")]
        [SerializeField, Min(0), Tooltip("Determines how fast the boat walks")]
        private float movementSpeed = 20;

        private Vector3 targetPosition;

        [SerializeField]
        private float targetSpeed;
        
        [SerializeField, Min(0), Tooltip("Determines how fast the boat speeds up")]
        private float movementAcceleration = 0.01f;

        [SerializeField, Min(0), Tooltip("Determines how fast the boat slows down")]
        private float movementDeceleration = 0.005f;

        private void UpdateMove(float elapsed)
        {
            var amount = 0f;

            if (this.direction.y > 0)
                amount = 2;

            if (this.direction.y < 0)
                amount = -0.5f;

            if (this.direction.x != 0)
                amount = 1;

            amount *= this.movementSpeed;

            this.targetSpeed = this.targetSpeed < amount
                ? Mathf.Clamp(this.targetSpeed + this.movementAcceleration, float.MinValue, amount)
                : Mathf.Clamp(this.targetSpeed - this.movementDeceleration, amount, float.MaxValue);

            this.targetPosition = this.transform.position + (this.transform.forward * this.targetSpeed);
            this.transform.position = this.transform.position.LerpAll(this.targetPosition, elapsed);
        }

        #endregion

        #region Turn

        [Header("Turn")]
        [SerializeField]
        private float turningSpeed = 1;
        private Vector2 direction;

        private void UpdateTurn(float elapsed)
        {
            if (this.direction.x == 0)
                return;

            var amount = this.direction.x * this.turningSpeed;

            this.transform.Rotate(amount * elapsed * Vector3.up);
        }

        #endregion

        #region Wheel

        [Header("Wheel")]
        [SerializeField, Tooltip("The object that will rotate upon the turning")]
        private WheelSteer wheel;
        private Vector2 targetWheel;

        private void TurnWheel(float angle)
        {
            if (this.wheel == null)
                return;

            this.wheel.AddAngle(angle);
        }

        private void UpdateWheel(float elapsed)
        {
            if (this.targetWheel.x == 0)
                return;

            var angle = Mathf.Sign(this.targetWheel.x) * elapsed;

            this.TurnWheel(angle);
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
        protected override void OnMove(Vector2 direction)
        {
            this.targetWheel = direction;
            this.direction = direction;
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut()
        {
            foreach (var item in this.icons)
                item.SetActive(true);
        }

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            foreach (var item in this.icons)
                item.SetActive(false);

            this.direction = Vector2.zero;
            this.targetWheel = Vector2.zero;
        }

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            if (this.isEnabled)
            {
                this.UpdateWheel(elapsed);
                this.UpdateTurn(elapsed);
            }
            
            this.UpdateMove(elapsed);
        }

        #endregion
    }
}