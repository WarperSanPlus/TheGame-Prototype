using UnityEngine;

namespace Controllers
{
    public class PlayerController : Controller
    {
        [Header("Move")]
        [SerializeField] float movementSpeed = 20;
        CharacterController cc;
        Animator animator;

        private Vector3 direction = Vector3.zero;

        void Start()
        {
            this.cc = this.GetComponent<CharacterController>();
            this.animator = this.GetComponent<Animator>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            CameraMovement.Instance.SetController(this);
        }

        private void Update()
        {
            _ = this.MoveSelf(this.direction, this.movementSpeed, Time.deltaTime);

            if (this.target != null)
            {
                this.transform.rotation = Quaternion.Euler(0, this.target.eulerAngles.y, 0);
            }
        }

        private CollisionFlags? MoveSelf(Vector3 facing, float speed, float elapsed)
        {
            if (this.target == null || this.cc == null)
                return null;

            var direction = (this.target.transform.forward * facing.y) + (this.target.transform.right * facing.x);

            var isWalking = direction.magnitude > 0;
            if (isWalking)
                direction.y = 0;

            if (this.animator != null)
                this.animator.SetBool("Walking", isWalking);

            direction = direction.normalized * speed;
            direction += Vector3.down;

            return this.cc.Move(direction * elapsed);
        }

        #region Controller

        /// <inheritdoc/>
        protected override void OnMove(Vector2 direction) => this.direction = direction;

        #endregion
    }
}