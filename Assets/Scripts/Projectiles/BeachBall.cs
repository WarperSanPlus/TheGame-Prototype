using Singletons;
using UnityEngine;

namespace Projectiles
{
    public class BeachBall : Projectile
    {
        public float force = 0;

        private bool hasSplashed = false;

        [SerializeField]
        private GameObject splashPrefab;

        private void Update()
        {
            // Stop checking if already splashed
            if (this.hasSplashed)
                return;

            var pos = this.transform.position;
            var y = OceanManager.GetHeight(pos);

            // Skip if above water
            if (y < pos.y)
                return;

            this.hasSplashed = true;

            var splash = ObjectPools.ObjectPool.GetObject(this.splashPrefab);

            // If splash invalid, skip
            if (splash == null)
                return;

            splash.transform.position = pos;

            if (splash.TryGetComponent(out ParticleSystem particle))
            {
                // Burst count
                var burst = particle.emission.GetBurst(0);
                var count = 10;

                if (this.force > 0.5f)
                    count += Mathf.FloorToInt(40 * this.force);

                burst.count = count;
                particle.emission.SetBurst(0, burst);

                // Force Over Lifetime
                var forceOverLifetime = particle.forceOverLifetime;
                var yForce = new ParticleSystem.MinMaxCurve
                {
                    constant = this.force > 0.5f ? Random.Range(Mathf.Lerp(-10, -5, this.force), Mathf.Lerp(-5, -1, this.force)) : Random.Range(-10, -5)
                };

                forceOverLifetime.y = yForce;
            }
        }

        /// <inheritdoc/>
        public override void OnReset() 
        {
            this.hasSplashed = false;
        }
    }
}