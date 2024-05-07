using UnityEngine;

namespace Interfaces
{
    public interface Interactable
    {
        public void OnClick();

        /// <summary>
        /// Tries to find a target and interacts with it
        /// </summary>
        public static void TryInteract(Vector3 position, Vector3 direction, float? maxDistance = null)
        {
            if (!CanInteract(position, direction, out var interactable, maxDistance))
                return;

            Interact(interactable);
        }

        public static bool CanInteract(Vector3 position, Vector3 direction, out Interactable interactable, float? maxDistance = null)
        {
            if (!Physics.Raycast(position, direction, out var hit, maxDistance ?? float.MaxValue))
            {
                interactable = null;
                return false;
            }

            return hit.collider.gameObject.TryGetComponent(out interactable);
        }

        /// <summary>
        /// Starts an interaction with the given target
        /// </summary>
        public static void Interact(Interactable target) => target.OnClick();
    }
}