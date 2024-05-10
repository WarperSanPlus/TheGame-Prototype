using Controllers;
using System.Collections.Generic;
using System.Linq;

namespace Singletons
{
    /// <summary>
    /// Class that manages the movement between multiple controllers
    /// </summary>
    public class ControllerManager : Singleton<ControllerManager>
    {
        /// <summary>
        /// List of the controllers used
        /// </summary>
        private static readonly Stack<Controller> stack = new();

        /// <summary>
        /// Current controller being used
        /// </summary>
        private static Controller ActiveController => stack.Count > 0 ? stack.Peek() : null;

        /// <summary>
        /// Switches to the given controller
        /// </summary>
        /// <param name="controller">Controller to switch to</param>
        public static void SwitchTo(Controller controller, bool teleportToTarget = true)
        {
            // If exists, switch out current
            if (ActiveController != null)
                ActiveController.SwitchOut();

            // Cancel if given not found
            if (controller == null)
                return;

            // Switch in the given
            controller.SwitchIn();
            stack.Push(controller);

            // Set the controller to the given
            CameraMovement.Instance.SetController(controller, teleportToTarget);
        }

        /// <summary>
        /// Replaces the current controller with the given controller
        /// </summary>
        public static void ReplaceCurrent(Controller controller, bool teleportToTarget = true)
        {
            // Skip if current not found
            if (ActiveController == null)
                return;

            // Remove current
            var current = stack.Pop();
            current.SwitchOut();

            // Switch to given
            SwitchTo(controller, teleportToTarget);
        }

        /// <summary>
        /// Switches to the previous controller
        /// </summary>
        public static void BackTo(bool teleportToTarget = true)
        {
            if (stack.Count <= 1)
                return;

            var previous = stack.Skip(1).First();
            ReplaceCurrent(previous, teleportToTarget);
        }
    }
}