using UnityEngine;
using UnityEngine.InputSystem;

namespace Singletons
{
    /// <summary>
    /// Class that manages the inputs of the player
    /// </summary>
    public class InputMaster : Singleton<InputMaster>
    {
        #region Delegate

        public delegate void LookEvent(Vector2 direction);
        public delegate void MoveEvent(Vector2 direction);
        public delegate void FireEvent();
        public delegate void InteractEvent();

        #endregion

        #region Events

        public event LookEvent OnLook;
        public event MoveEvent OnMove;
        public event FireEvent OnFireStart;
        public event FireEvent OnFireEnd;
        public event InteractEvent OnInteract;

        #endregion

        #region Callback

        public void Look(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();

            this.OnLook?.Invoke(direction);
        }

        public void Move(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();

            this.OnMove?.Invoke(direction);
        }

        public void Fire(InputAction.CallbackContext context)
        {
            if (context.started)
                this.OnFireStart?.Invoke();
            else if (context.canceled)
                this.OnFireEnd?.Invoke();
        }

        public void Interact(InputAction.CallbackContext context)
        {
            this.OnInteract?.Invoke();
        }

        #endregion
    }
}