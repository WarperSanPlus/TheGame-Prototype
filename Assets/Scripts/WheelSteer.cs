using UnityEngine;

public class WheelSteer : MonoBehaviour
{
    [SerializeField, Range(-1.5f, 1.5f)]
    private float angle = 0;

    [SerializeField, Tooltip("GameObject that will spin")]
    private GameObject wheel;

    // Update is called once per frame
    void Update() => this.RotateWheel(Time.deltaTime);

    private void RotateWheel(float elapsed) {

        var rotationAmount = Mathf.Sign(this.angle) * Mathf.Pow(Mathf.Abs(this.angle), 2) * elapsed * 50;

        this.wheel.transform.Rotate(Vector3.forward, rotationAmount);
    }

    public void AddAngle(float angle) => this.angle = Mathf.Clamp(this.angle + angle, -1.5f, 1.5f);
}
