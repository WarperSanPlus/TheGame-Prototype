using UnityEngine;

public class MoveByVector : MonoBehaviour
{
    [SerializeField, Tooltip("Direction of the movement")]
    private Vector3 direction;

    [Tooltip("By how much the movement is sped up")]
    public float speed;

    [SerializeField]
    private bool useLocal = false;

    private Vector3 GetDirection(float elapsed) => this.speed * elapsed * this.direction.normalized;

    /// <summary>
    /// Updates the position
    /// </summary>
    /// <param name="elapsed">Time passed since the last call</param>
    private void UpdatePosition(float elapsed)
    {
        var direction = this.GetDirection(elapsed);
        this.transform.Translate(direction, this.useLocal ? Space.Self : Space.World);
    }

    #region MonoBehaviour

    /// <inheritdoc/>
    private void FixedUpdate() => this.UpdatePosition(Time.fixedDeltaTime);

    #endregion

    #region Gizmos

    /// <inheritdoc/>
    private void OnDrawGizmosSelected()
    {
        if (!this.enabled)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position, this.transform.position + (this.GetDirection(Time.fixedDeltaTime) * 10));
    }

    #endregion Gizmos
}