using Interfaces;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour 
{
    /// <inheritdoc/>
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out ICollidable collider))
            return;

        collider.OnCollision(this);
    }

    public void Despawn() 
    {
        this.gameObject.SetActive(false);
    }
}