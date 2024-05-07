using Controllers;
using System.Collections.Generic;

namespace Singletons
{
    public class ControllerManager : Singleton<ControllerManager>
    {
        private static readonly Stack<Controller> stack = new();

        private static Controller ActiveController => stack.Count > 0 ? stack.Peek() : null;

        public static void SwitchTo(Controller controller, bool teleportToTarget = true)
        {
            if (ActiveController != null)
                ActiveController.SwitchOut();

            if (controller == null)
                return;

            controller.SwitchIn();
            stack.Push(controller);

            CameraMovement.Instance.SetController(controller, teleportToTarget);
        }

        public static void BackTo(bool teleportToTarget = true) 
        {
            if (ActiveController == null)
                return;

            stack.Pop().SwitchOut();
            SwitchTo(ActiveController, teleportToTarget);
        }
    }
}