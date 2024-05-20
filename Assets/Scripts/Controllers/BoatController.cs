using Extensions;
using System.Collections.Generic;
using UnityEngine;


namespace Controllers
{
    /// <summary>
    /// Controller that manages how the boat behaves
    /// </summary>
    public class BoatController : Controller
    {
        
        #region Move

        [Header("Move")]
        [SerializeField, Min(0), Tooltip("Determines how fast the boat walks")]
        private float movementSpeed = 20;
        
        [SerializeField, Min(0), Tooltip("Determines how fast the boat speeds up")]
        private float movementAcceleration = 0.01f;

        [SerializeField, Min(0), Tooltip("Determines how fast the boat slows down")]
        private float movementDeceleration = 0.005f;

        [SerializeField, Min(0), Tooltip("Determines the offset of the boat from the wave height")]
        private float waveOffset = 0;

        private Vector3 targetPosition;
        private float currentSpeed;

        /// <summary>
        /// Updates the movement of the boat
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateMove(float elapsed)
        {
            var speed = GetSpeedMultiplier(this.direction) * this.movementSpeed;

            // Lerp the current speed to the wanted speed
            this.currentSpeed = this.currentSpeed < speed
                ? Mathf.Clamp(this.currentSpeed + this.movementAcceleration, float.MinValue, speed)
                : Mathf.Clamp(this.currentSpeed - this.movementDeceleration, speed, float.MaxValue);

            // Updates the wanted position
            this.targetPosition = this.transform.position + (this.transform.forward * this.currentSpeed);
            this.targetPosition.y = Singletons.OceanManager.GetHeight(this.targetPosition, this.waveOffset);

            // Lerps to the position
            var newPosition = this.transform.position.LerpAll(this.targetPosition, elapsed);

            // Update positions
            var diff = newPosition - this.transform.position;
            this.transform.position = newPosition;
            this.UpdateAboardPosition(diff);
        }

        /// <summary>
        /// Gets the speed multiplier depending of the direction of the movement
        /// </summary>
        /// <param name="direction">Direction of the movement</param>
        /// <returns>Multiplier of the speed</returns>
        private static float GetSpeedMultiplier(Vector2 direction)
        {
            // Double speed if going forwards
            if (direction.y > 0)
                return 2;

            // Half speed if going backwards
            if (direction.y < 0)
                return -0.5f;

            // Regular speed if turning
            if (direction.x != 0)
                return 1;

            // No speed if not moving
            return 0;
        }

        #endregion

        #region Aboard

        private readonly List<Rigidbody> aboardItems = new();

        /// <summary>
        /// Updates the position of all the items aboard
        /// </summary>
        private void UpdateAboardPosition(Vector3 movement)
        {
            foreach (var item in this.aboardItems)
            {
                if (item == null)
                    continue;

                item.MovePosition(item.position + movement);
            }
        }

        /// <summary>
        /// Updates the rotation of all the items aboard
        /// </summary>
        private void UpdateAboardRotation(Vector3 rotation)
        {
            foreach (var item in this.aboardItems)
            {
                if (item == null)
                    continue;

                item.transform.Rotate(rotation);
            }
        }

        #endregion

        #region Turn

        [Header("Turn")]
        [SerializeField, Min(0), Tooltip("Determines how fast the boat can turn")]
        private float turningSpeed = 1;
        private Vector2 direction;

        /// <summary>
        /// Updates the rotation of the boat
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateTurn(float elapsed)
        {
            // Skip if no turn
            if (this.direction.x == 0)
                return;

            var amount = this.direction.x * this.turningSpeed;

            this.transform.Rotate(amount * elapsed * Vector3.up);
            this.UpdateAboardRotation(amount * elapsed * Vector3.up);
        }

        #endregion

        #region Wheel

        [Header("Wheel")]
        [SerializeField, Tooltip("The object that will rotate upon the turning")]
        private WheelSteer wheel;
        private Vector2 targetWheel;

        /// <summary>
        /// Turns the wheel by the given angle
        /// </summary>
        /// <param name="angle">Angle to turn the wheel by</param>
        private void TurnWheel(float angle)
        {
            if (this.wheel == null)
                return;

            this.wheel.AddAngle(angle);
        }

        /// <summary>
        /// Updates the rotation of the wheel
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
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
        protected override void OnMove(Vector2 direction)
        {
            this.targetWheel = direction;
            this.direction = direction;
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut() 
        {
            this.SetCursorLock(false);
            this.SetIconsVisibility(true);

            this.direction = Vector2.zero;
            this.targetWheel = Vector2.zero;
        }

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            this.SetCursorLock(true);
            this.SetIconsVisibility(false);
        }

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            // Only update when enabled
            if (this.IsEnabled)
            {
                this.UpdateWheel(elapsed);
                this.UpdateTurn(elapsed);
            }
        }

        /// <inheritdoc/>
        protected override void OnFixedUpdate(float elapsed)
        {
            this.UpdateMove(elapsed);
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void OnTriggerEnter(Collider other) 
        {
            if (!other.gameObject.TryGetComponent(out Rigidbody rb))
                return;

            this.aboardItems.Add(rb);
        }

        /// <inheritdoc/>
        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out Rigidbody rb))
                return;

            _ = this.aboardItems.Remove(rb);
        }

        #endregion

        
    }
}