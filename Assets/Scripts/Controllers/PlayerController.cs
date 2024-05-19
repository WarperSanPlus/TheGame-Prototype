using Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    /// <summary>
    /// Controller that manages how the player behaves
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : Controller
    {
        #region Cursor 

        [Header("Cursor")]
        [SerializeField, Tooltip("Determines the sprite to use when an interaction is possible")]
        private Sprite interactCursor;

        [SerializeField, Tooltip("Determines the sprite to use when no interaction is available")]
        private Sprite normalCursor;

        [SerializeField, Tooltip("Image that represents the cursor")]
        private Image cursor;

        [SerializeField, Min(0), Tooltip("Determines how far the player can interact with things")]
        private float interactRange = 0;

        /// <summary>
        /// Updates the cursor depending on the possible interactions
        /// </summary>
        private void UpdateCursor() 
        {
            // If cursor invalid, skip
            if (this.cursor == null)
                return;

            if (Interfaces.Interactable.CanInteract(this.cameraAnchor.position, this.cameraAnchor.forward, this.interactRange))
            {
                this.cursor.sprite = this.interactCursor;
                this.cursor.rectTransform.sizeDelta = new Vector2(50, 50);
            }
            else
            {
                this.cursor.sprite = this.normalCursor;
                this.cursor.rectTransform.sizeDelta = new Vector2(10, 10);
            }
        }

        /// <summary>
        /// Sets the cursor's visibility to the given visibility
        /// </summary>
        /// <param name="visible">Will the cursor be visible or not?</param>
        private void SetCursor(bool visible)
        {
            if (this.cursor == null)
                return;

            this.cursor.enabled = visible;
        }

        #endregion

        #region Move

        [Header("Move")]
        [SerializeField, Tooltip("Determines how fast the player can move")]
        private float movementSpeed = 20;

        private Rigidbody rb;
        private Animator animator;

        private Vector3 direction;

        /// <summary>
        /// Updates the movement of the player
        /// </summary>
        /// <param name="facing">Direction of the movement</param>
        /// <param name="speed">Speed of the movement</param>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateMove(Vector3 facing, float speed, float elapsed)
        {
            // Skip if anchor or rigidbody not found
            if (this.cameraAnchor == null || this.rb == null)
                return;

            var direction = (this.cameraAnchor.transform.forward * facing.y) + (this.cameraAnchor.transform.right * facing.x);

            var isWalking = direction.magnitude > 0;
            
            // Prevent walking in the air
            if (isWalking)
                direction.y = 0;

            // Update animator
            if (this.animator != null)
                this.animator.SetBool("Walking", isWalking);

            // Modify the direction
            direction = direction.normalized * speed;
            //direction += Vector3.down;

            // Move the character controller
            this.rb.MovePosition(this.rb.position + direction * elapsed);
        }

        #endregion

        #region Respawn

        [Header("Respawn")]
        [SerializeField, Tooltip("Determines where the player respawns")]
        private Transform spawnPoint;

        /// <summary>
        /// Respawns the player
        /// </summary>
        public void Respawn()
        {
            // Teleport back to origin
            var position = this.spawnPoint == null ? Vector3.zero : this.spawnPoint.position;
            
            this.transform.position = position;
        }

        #endregion

        #region Render

        [Header("Render")]
        [SerializeField, Tooltip("GameObject to hide when the controller is no longer used")]
        private GameObject hideWhenOut;

        /// <summary>
        /// Sets the visibility of the target object
        /// </summary>
        /// <param name="visible">Should the target be visible or not?</param>
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
            // Get components
            this.animator = this.GetComponent<Animator>();
            this.rb = this.GetComponent<Rigidbody>();

            // Start with this controller
            ControllerManager.SwitchTo(this);
        }

        /// <inheritdoc/>
        protected override void OnUpdate(float elapsed)
        {
            if (this.cameraAnchor != null)
            {
                this.transform.rotation = Quaternion.Euler(0, this.cameraAnchor.rotation.eulerAngles.y, 0);
            }

            this.UpdateCursor();
        }

        /// <inheritdoc/>
        protected override void OnFixedUpdate(float elapsed) 
        {
            this.UpdateMove(this.direction, this.movementSpeed, elapsed);
        }

        /// <inheritdoc/>
        protected override void OnMove(Vector2 direction) => this.direction = direction;

        /// <inheritdoc/>
        protected override void OnFireStart() => Interfaces.Interactable.TryInteract(this.cameraAnchor.position, this.cameraAnchor.forward, this.interactRange);

        /// <inheritdoc/>
        protected override void OnSwitchIn()
        {
            // Update cursor
            this.SetCursor(true);
            this.SetCursorLock(true);

            // Set visible
            this.SetVisible(true);
            
            // Reset direction
            this.direction = Vector2.zero;
        }

        /// <inheritdoc/>
        protected override void OnSwitchOut()
        {
            // Update cursor
            this.SetCursor(false);
            this.SetCursorLock(false);

            // Set visible
            this.SetVisible(false);
        }

        #endregion
    }
}