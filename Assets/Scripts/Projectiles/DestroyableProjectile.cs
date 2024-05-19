using Interfaces;
using Projectiles;
using UnityEngine;

public class DestroyableProjectile : MonoBehaviour, ICollidable
{
    [SerializeField]
    private GameObject particles;

    /// <inheritdoc/>
    public void OnCollision(Projectile source)
    {
        // Can't be destroyed by other than player
        if (source is not BeachBall beach)
            return;

        // Can't be destroyed by weak ball
        if (beach.force < 0.75f)
            return;


        var particles = ObjectPools.ObjectPool.GetObject(this.particles);

        if (particles != null && particles.TryGetComponent(out ParticleSystem particleSystem))
        {
            particles.transform.position = this.transform.position;
            particles.SetActive(true);
            particleSystem.Play();
        }

        this.gameObject.SetActive(false);
    }
}
