using Extensions;
using UnityEngine;

public class GolemAnimationEvents : MonoBehaviour
{
    #region Throw

    [SerializeField, Tooltip("Prefab to throw")]
    private GameObject throwProjectile;
    
    [SerializeField, Tooltip("Initial position of the projectile")]
    private Transform throwSource;

    [SerializeField, Tooltip("Force applied to the projectile upon launch")]
    private Vector3 throwForce;

    public Transform throwTarget;

    public void ExecuteThrow()
    {
        // If source is invalid, skip
        if (this.throwSource == null)
            return;

        // If target is invalid, skip
        if (this.throwTarget == null)
            return;

        var obj = ObjectPools.ObjectPool.GetObject(this.throwProjectile);

        // If projectile invalid, skip
        if (obj == null)
            return;

        obj.transform.position = this.throwSource.position;
        //obj.transform.forward = this.transform.forward;

        if (obj.TryGetComponent(out Projectiles.Projectile projectile) && obj.TryGetComponent(out Rigidbody rb))
        {
            rb.velocity = projectile.GetLaunch(this.throwTarget.position, 10);
        }
    }

    #endregion

    public void ExecuteStomp()
    {

    }
}
