namespace Interfaces
{
    /// <summary>
    /// Defines the objects that want to be notify when a collision happened
    /// </summary>
    public interface ICollidable
    {
        /// <summary>
        /// Called when a projectile collided with this object
        /// </summary>
        /// <param name="source">Projectile that caused the collision</param>
        public void OnCollision(Projectiles.Projectile source);
    }
}