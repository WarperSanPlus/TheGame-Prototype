using Interfaces;
using UnityEngine;

/// <summary>
/// Juan
/// </summary>
public class Juan : MonoBehaviour, ICollidable
{
    [SerializeField]
    private Animator juanAnimator;

    private bool hasDied = false;

    #region ICollidable

    /// <inheritdoc/>
    public void OnCollision(Projectile source)
    {
        if (!this.hasDied)
        {
            this.juanAnimator.SetTrigger("Death");
            this.GetComponent<Collider>().enabled = false;
        }

        this.hasDied = true;
    }

    #endregion
}
