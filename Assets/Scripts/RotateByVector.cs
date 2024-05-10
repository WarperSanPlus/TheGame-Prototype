using UnityEngine;

public class RotateByVector : MonoBehaviour
{
    [SerializeField, Tooltip("Direction of the rotation")]
    private Vector3 direction = Vector3.forward;

    [Tooltip("By how much the rotation is sped up")]
    public float speed = 1f;

    private Vector3 GetRotation(float elapsed) => this.speed * elapsed * this.direction.normalized;

    /// <summary>
    /// Updates the rotation
    /// </summary>
    /// <param name="elapsed">Time passed since the last call</param>
    private void UpdateRotation(float elapsed)
    {
        var rotation = this.GetRotation(elapsed);
        this.transform.Rotate(rotation);
    }

    #region MonoBehaviour

    /// <inheritdoc/>
    private void FixedUpdate() => this.UpdateRotation(Time.fixedDeltaTime);

    #endregion
}