using UnityEngine;

//V1
//public class CameraMovement : MonoBehaviour
//{
//    [SerializeField] float camSpeed = 1.0f;
//    public Transform trackedObject;

//    // Update is called once per frame
//    void Update()
//    {
//        if (this.trackedObject != null)
//            this.LerpToTarget(this.trackedObject, Time.deltaTime);
//    }
//    void LerpToTarget(Transform target, float duration)
//    {
//        Vector3 pos = this.transform.position;
//        pos.x = Mathf.Lerp(pos.x, target.position.x, duration);
//        pos.y = Mathf.Lerp(pos.y, target.position.y, duration);
//        pos.z = Mathf.Lerp(pos.z, target.position.z, duration);

//        this.transform.position = pos;

//        //this.transform.eulerAngles = new Vector3(target.eulerAngles.x, target.eulerAngles.y, 0);

//        Vector3 originAngle = this.transform.eulerAngles;
//        Vector3 targetAngle = target.eulerAngles;

//        this.transform.eulerAngles = new Vector3(
//            Mathf.LerpAngle(originAngle.x, targetAngle.x, duration),
//            Mathf.LerpAngle(originAngle.y, targetAngle.y, duration), 
//            0);
//    }
//    void SetTarget(Transform target)
//    {

//    }
//}

//V2 
public class CameraMovement : MonoBehaviour
{
    [SerializeField] float camSpeed = 1.0f;
    public Transform trackedObject;
    bool isLerping = true;
    float lerpThreshold = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (this.trackedObject != null && this.isLerping)
        {
            this.LerpToTarget(this.trackedObject, Time.deltaTime);
            if (Vector3.Distance(this.transform.position, this.trackedObject.position) < this.lerpThreshold)
                this.isLerping = false;
        }
        else
        {
            this.FollowTarget(this.trackedObject);
        }
    }

    void LerpToTarget(Transform target, float duration)
    {
        Vector3 pos = this.transform.position;
        pos.x = Mathf.Lerp(pos.x, target.position.x, this.camSpeed * duration);
        pos.y = Mathf.Lerp(pos.y, target.position.y, this.camSpeed * duration);
        pos.z = Mathf.Lerp(pos.z, target.position.z, this.camSpeed * duration);

        this.transform.position = pos;

        Vector3 originAngle = this.transform.eulerAngles;
        Vector3 targetAngle = target.eulerAngles;

        this.transform.eulerAngles = new Vector3(
            Mathf.LerpAngle(originAngle.x, targetAngle.x, this.camSpeed * duration),
            Mathf.LerpAngle(originAngle.y, targetAngle.y, this.camSpeed * duration),
            0);
    }

    void FollowTarget(Transform target)
    {
        if (target != null)
        {
            this.transform.position = target.position;
            this.transform.rotation = target.rotation;
        }
    }
}
