using Projectiles;
using UnityEngine;

public class Target : MonoBehaviour, Interfaces.ICollidable
{
    public void OnCollision(Projectile source)
    {
        source.Despawn();
    }
}