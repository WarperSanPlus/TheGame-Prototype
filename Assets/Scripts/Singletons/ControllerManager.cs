using Controllers;
using UnityEngine;

namespace Singletons
{
    public class ControllerManager : Singleton<ControllerManager>
    {
        private Controller activeController = null;

        public static void SwitchTo(Controller controller, bool teleportToTarget = true)
        {
            if (Instance.activeController != null)
                Instance.activeController.enabled = false;
            controller.enabled = true;

            Instance.activeController = controller;
            CameraMovement.Instance.SetController(controller, teleportToTarget);
        }
    }
}