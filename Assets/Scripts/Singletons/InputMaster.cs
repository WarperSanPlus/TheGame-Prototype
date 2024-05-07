using UnityEngine;
using UnityEngine.InputSystem;

namespace Singletons
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputMaster : Singleton<InputMaster>
    {
        public delegate void LookEvent(Vector2 direction);
        public event LookEvent OnLook;

        public void Look(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();

            this.OnLook?.Invoke(direction);
        }

        public delegate void MoveEvent(Vector2 direction);
        public event MoveEvent OnMove;

        public void Move(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();

            this.OnMove?.Invoke(direction);
        }

        public delegate void FireEvent();
        public event FireEvent OnFire;

        public void Fire(InputAction.CallbackContext context)
        {
            if (context.started)
                this.OnFire?.Invoke();
        }
        
        public delegate void InteractEvent();
        public event InteractEvent OnInteract;

        public void Interact(InputAction.CallbackContext context) 
        {
            this.OnInteract?.Invoke();
        }
    }
}