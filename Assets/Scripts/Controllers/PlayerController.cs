using Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    /// <summary>
    /// Controller that manages how the player behaves
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : Controller
    {
        #region Cursor 

        [Header("Cursor")]
        [SerializeField, Tooltip("Determines the sprite to use when an interaction is possible")]
        private Sprite interactCursor;

        [SerializeField, Tooltip("Determines the sprite to use when no interaction is available")]
        private Sprite normalCursor;

        [SerializeField]
        private Image cursor;

        [SerializeField, Min(0), Tooltip("Determines how far the player can interact with things")]
        private float interactRange = 0;

        private void UpdateCursor() => this.cursor.sprite = Interfaces.Interactable.CanInteract(
            this.transform.position,
            this.target.forward,
            out _,
            this.interactRange
        ) ? this.interactCursor : this.normalCursor;

        private void SetCursor(bool visible)
        {
            if (this.cursor != null)
                this.cursor.enabled = visible;
        }

        #endregion

        #region Move

        [Header("Move")]
        [SerializeField, Tooltip("Determines how fast the player can move")]
        private float movementSpeed = 20;

        private CharacterController cc;
        private Animator animator;

        private Vector3 direction;

        private void UpdateMove(Vector3 facing, float speed, float elapsed)
        {
            if (this.target == null || this.cc == null)
                return;

            var direction = (this.target.transform.forward * facing.y) + (this.target.transform.right * facing.x);

            var isWalking = direction.magnitude > 0;
            if (isWalking)
                direction.y = 0;

            if (this.animator != null)
                this.animator.SetBool("Walking", isWalking);

            direction = direction.normalized * speed;
            direction += Vector3.down;

            _ = this.cc.Move(direction * elapsed);
        }

        #endregion

        #region Render

        [Header("Render")]
        [SerializeField, Tooltip("GameObject to hide when the controller is no longer used")]
        private GameObject hideWhenOut;

        private void SetVisible(bool visible)
        {
            if (this.hideWhenOut == null)
                return;

            this.hideWhenOut.SetActive(visible);
        }

        #endregion

        #region Controller

        /// <inheritdoc/>
        protected override void OnStart()
        {
            this.cc = this.GetComponent<CharacterController>();
            this.animator = this.GetComponent<Animator>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            ControllerManager.SwitchTo(this);
        }

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            this.UpdateMove(this.direction, this.movementSpeed, elapsed);

            if (this.target != null)
            {
                this.transform.rotation = Quaternion.Euler(0, this.target.rotation.eulerAngles.y, 0);
            }

            this.UpdateCursor();
        }

        /// <inheritdoc/>
        protected override void OnMove(Vector2 direction) => this.direction = direction;

        /// <inheritdoc/>
        protected override void OnFire() => Interfaces.Interactable.TryInteract(this.transform.position, this.target.forward, this.interactRange);

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            this.SetCursor(true);
            this.SetVisible(true);
            this.direction = Vector2.zero;
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut()
        {
            this.SetCursor(false);
            this.SetVisible(false);
        }

        #endregion
    }
}