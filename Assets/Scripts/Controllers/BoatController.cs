using Extensions;
using Singletons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class BoatController : Controller
    {
        [Header("Move")]
        [SerializeField] float movementSpeed = 20;
        Vector3 move = Vector3.zero;

        public WheelSteer wheel;
        private Vector2 targetWheel;
        private Vector2 direction;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {

            if (this.targetWheel.x != 0)
            {
                var angle = Mathf.Sign(this.targetWheel.x) * Time.deltaTime;
                this.wheel.AddAngle(angle);
            }

            if (this.direction.x != 0)
            {
                this.TurnBoat(this.direction.x, Time.deltaTime);
            }

            this.MoveBoat(1, Time.deltaTime);
            this.transform.position = this.transform.position.LerpAll(this.targetPosition, Time.deltaTime);
        }

        #region Controller

        protected override void OnMove(Vector2 direction)
        {
            this.targetWheel = direction;
            this.direction = direction;
        }

        #endregion

        [SerializeField]
        private float turningSpeed = 1;

        private void TurnBoat(float amount, float elapsed)
        {
            amount *= this.turningSpeed;

            this.transform.Rotate(amount * elapsed * Vector3.up);
        }

        private Vector3 targetPosition;

        private void MoveBoat(float amount, float elapsed)
        {
            this.targetPosition = this.transform.position + this.transform.forward * amount;
        }
    }
}