using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class WorldIcon : MonoBehaviour
{
    [SerializeField, Tooltip("Target of the icon")]
    private Transform target;

    [SerializeField, Tooltip("Distance where the icon starts to fade out")]
    private float minDistance = 1;

    [SerializeField, Tooltip("Distance where the icon starts to fade out")]
    private float maxDistance = 10;

    [SerializeField, Tooltip("Sprite to display")]
    private Sprite sprite;

    private CanvasGroup group;

    /// <summary>
    /// Updates the rotation of the icon
    /// </summary>
    private void UpdateRotation()
    {
        // Skip if target invalid
        if (this.target == null)
            return;

        // Rotate towards target
        this.transform.LookAt(this.target);
    }

    /// <summary>
    /// Updates the alpha of the icon
    /// </summary>
    private void UpdateAlpha()
    {
        // Skip if target or group invalid
        if (this.target == null || this.group == null)
            return;

        var alpha = 1f;
        var distance = Vector3.Distance(this.transform.position, this.target.transform.position);

        // Add offset
        distance -= 0.35f;

        // Calculate alpha depending on the distance
        if (distance < this.minDistance)
            alpha = distance / this.minDistance;

        if (distance > this.maxDistance)
            alpha = this.maxDistance / distance;

        this.group.alpha = alpha;
    }

    #region MonoBehaviour

    /// <inheritdoc/>
    private void Start()
    {
        this.target = this.target != null ? this.target : Camera.main.transform;
        this.group = this.GetComponent<CanvasGroup>();

        this.GetComponentInChildren<Image>().sprite = this.sprite;
    }

    /// <inheritdoc/>
    private void Update()
    {
        this.UpdateRotation();
        this.UpdateAlpha();
    }

    #endregion

    #region Gizmos

    /// <inheritdoc/>
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this.minDistance);

        Gizmos.color = Color.green;    
        Gizmos.DrawWireSphere(this.transform.position, this.maxDistance);
    }

    #endregion
}
