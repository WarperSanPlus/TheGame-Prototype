using Extensions;
using UnityEngine;

public class CameraMovement : Singletons.Singleton<CameraMovement>
{
    private const float LERP_THRESHOLD = 0.5f;

    [SerializeField] float camSpeed = 1.0f;
    [SerializeField] private Transform trackedObject;
    public bool isLerping = true;

    private void UpdateMovement(float elapsed)
    {
        if (this.trackedObject == null)
            return;

        var duration = this.isLerping ? this.camSpeed * elapsed : 1;

        if (this.isLerping && Vector3.Distance(this.transform.position, this.trackedObject.position) < LERP_THRESHOLD)
            this.isLerping = false;

        this.transform.LerpToTarget(this.trackedObject, duration);
    }

    public void SetController(Controllers.Controller controller, bool teleportToTarget = true) 
    {
        this.trackedObject = controller.target;
        this.isLerping = !teleportToTarget;

        this.UpdateMovement(0);
    }

    #region MonoBehaviour

    // Update is called once per frame
    void Update() => this.UpdateMovement(Time.deltaTime);

    #endregion
}