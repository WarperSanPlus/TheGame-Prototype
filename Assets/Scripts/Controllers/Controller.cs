using Extensions;
using Singletons;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Class that provides methods to use other controller indenpendly
    /// </summary>
    public abstract class Controller : MonoBehaviour
    {
        /// <summary>
        /// Called when the player moves
        /// </summary>
        protected virtual void OnMove(Vector2 direction) { }

        /// <summary>
        /// Called when the player presses the 'Fire' button
        /// </summary>
        protected virtual void OnFire() { }

        /// <summary>
        /// Called when the player presses the 'Interact' button
        /// </summary>
        protected virtual void OnInteract() => ControllerManager.BackTo(true);

        #region Look

        [Header("Look")]
        [Tooltip("Object that the camera will follow")]
        public Transform cameraAnchor;

        [SerializeField, Range(0, 15), Tooltip("Determines how fast the camera rotates")]
        private float sensitivity = 2.0f;

        [SerializeField, Tooltip("Max angles that this controller can rotate")]
        private Vector3 maxAngles;

        [SerializeField, Tooltip("Axis on which the angles are clamped")]
        private Vector3Int clampAxis;

        [SerializeField, Tooltip("Determines if the anchor rotates with it's parent or not")]
        private bool rotateWithController = false;

        /// <summary>
        /// Called when the player requests a rotation
        /// </summary>
        /// <param name="direction">Direction of the rotation</param>
        protected virtual void OnLook(Vector2 direction) => this.camDirection = direction;

        private Vector3 camRotation = Vector3.zero;
        private Vector3 camDirection;

        /// <summary>
        /// Updates the rotation of the target
        /// </summary>
        /// <param name="direction">Direction of the rotation</param>
        /// <param name="elapsed">Time since the last frame</param>
        private void UpdateRotation(Vector2 direction, float elapsed) => this.camRotation = this.cameraAnchor.ClampRotation(
            this.camRotation,
            elapsed * this.sensitivity * new Vector3(-direction.y, direction.x, 0),
            this.maxAngles,
            this.clampAxis,
            this.rotateWithController ? this.transform.eulerAngles : null
        );

        #endregion

        #region Switch

        [Header("Switch")]
        [SerializeField, Tooltip("Determines if, when this controller switches out, it disables itself")]
        private bool disableIfOut = true;

        /// <summary>
        /// Is this controller enabled or not?
        /// </summary>
        /// <remarks>
        /// This allows a controller to receive certain updates while being "disabled"
        /// </remarks>
        protected bool IsEnabled { get; private set; } = true;

        /// <summary>
        /// Starts using this controller
        /// </summary>
        public void SwitchIn()
        {
            // Subscribe all
            InputMaster.Instance.OnLook += this.OnLook;
            InputMaster.Instance.OnMove += this.OnMove;
            InputMaster.Instance.OnFire += this.OnFire;
            InputMaster.Instance.OnInteract += this.OnInteract;

            // Update enable states
            this.IsEnabled = true;
            this.enabled = true;

            // Call callback
            this.OnSwitchIn();
        }

        /// <summary>
        /// Stops using this controller
        /// </summary>
        public void SwitchOut()
        {
            // Unsubscribe all
            InputMaster.Instance.OnLook -= this.OnLook;
            InputMaster.Instance.OnMove -= this.OnMove;
            InputMaster.Instance.OnFire -= this.OnFire;
            InputMaster.Instance.OnInteract -= this.OnInteract;

            // Update enable states
            this.IsEnabled = false;
            if (this.disableIfOut)
                this.enabled = false;

            // Resets the direction
            this.camDirection = Vector3.zero;

            // Call callback
            this.OnSwitchOut();
        }

        /// <summary>
        /// Called when this controller is started to being used
        /// </summary>
        protected virtual void OnSwitchIn() { }

        /// <summary>
        /// Called when this controller is no longer being used
        /// </summary>
        protected virtual void OnSwitchOut() { }

        #endregion

        #region Icons

        [Header("Icons")]
        [SerializeField, Tooltip("All the icons that will be affected by SetIconsVisibility")]
        private GameObject[] icons;

        /// <summary>
        /// Sets the visiblity of the icons to the given state
        /// </summary>
        /// <param name="isVisible">Are the icons visible or not?</param>
        protected void SetIconsVisibility(bool isVisible)
        {
            foreach (var item in this.icons)
            {
                if (item == null)
                    continue;

                item.SetActive(isVisible);
            }
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        public void Start() => this.OnStart();

        /// <summary>
        /// Called when this controller is being started
        /// </summary>
        protected virtual void OnStart() { }

        /// <inheritdoc/>
        private void Update()
        {
            this.OnUpdate(Time.deltaTime);
            this.UpdateRotation(this.camDirection, Time.deltaTime);
        }

        /// <summary>
        /// Called when this controller is being updated
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        protected virtual void OnUpdate(float elapsed) { }

        /// <inheritdoc/>
        private void FixedUpdate() => this.OnFixedUpdate(Time.fixedDeltaTime);

        /// <summary>
        /// Called when this controller is being updated
        /// </summary>
        /// <param name="elapsed">Time passed since the last call</param>
        protected virtual void OnFixedUpdate(float elapsed) { }

        #endregion

        #region Utilities

        /// <summary>
        /// Sets the cursor lock state 
        /// </summary>
        /// <param name="isLocked">Is the cursor locked or not?</param>
        protected void SetCursorLock(bool isLocked)
        {
            if (isLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        #endregion
    }
}
