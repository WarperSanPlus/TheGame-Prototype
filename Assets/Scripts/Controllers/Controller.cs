using Extensions;
using Singletons;
using UnityEngine;

namespace Controllers
{
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
        public Transform target;

        [SerializeField, Range(0, 15), Tooltip("Determines how fast the camera rotates")]
        private float sensitivity = 2.0f;

        [SerializeField, Tooltip("Max angles that this controller can rotate")]
        private Vector3 maxAngles;

        [SerializeField, Tooltip("Axis on which the angles are clamped")]
        private Vector3Int clampAxis;

        [SerializeField]
        private bool rotateWithController = false;

        protected virtual void OnLook(Vector2 direction) => this.direction = direction;

        private Vector3 camRotation = Vector3.zero;
        private Vector3 direction;

        private void RotateCamera(Vector2 direction, float elapsed) => this.camRotation = this.target.ClampRotation(
            this.camRotation,
            elapsed * this.sensitivity * new Vector3(-direction.y, direction.x, 0),
            this.maxAngles,
            this.clampAxis,
            this.rotateWithController ? this.transform.eulerAngles : null
        );

        #endregion

        #region Switch

        [Header("Controller")]
        [SerializeField, Tooltip("Determines if, when this controller switches out, it disables itself")]
        private bool disableIfOut = true;

        protected bool isEnabled = true;

        public void SwitchIn()
        {
            // Subscribe all
            InputMaster.Instance.OnLook += this.OnLook;
            InputMaster.Instance.OnMove += this.OnMove;
            InputMaster.Instance.OnFire += this.OnFire;
            InputMaster.Instance.OnInteract += this.OnInteract;

            this.isEnabled = true;
            this.enabled = true;
            this.OnSwitchIn();
        }

        public void SwitchOut()
        {
            // Unsubscribe all
            InputMaster.Instance.OnLook -= this.OnLook;
            InputMaster.Instance.OnMove -= this.OnMove;
            InputMaster.Instance.OnFire -= this.OnFire;
            InputMaster.Instance.OnInteract -= this.OnInteract;

            this.isEnabled = false;
            if (this.disableIfOut)
                this.enabled = false;

            this.direction = Vector3.zero;

            this.OnSwitchOut();
        }

        /// <summary>
        /// Called when this controller is currently being used
        /// </summary>
        protected virtual void OnSwitchIn() { }

        /// <summary>
        /// Called when this controller is no longer being used
        /// </summary>
        protected virtual void OnSwitchOut() { }

        #endregion

        #region MonoBehaviour

        public void Start() => this.OnStart();

        /// <summary>
        /// Called when this controller is being started
        /// </summary>
        protected virtual void OnStart() { }

        private void Update() 
        {
            this.OnUpdate(Time.deltaTime);
            this.RotateCamera(this.direction, Time.deltaTime);
        }

        /// <summary>
        /// Called when this controller is being updated
        /// </summary>
        /// <param name="elapsed"></param>
        protected virtual void OnUpdate(float elapsed) { }

        #endregion
    }
}
