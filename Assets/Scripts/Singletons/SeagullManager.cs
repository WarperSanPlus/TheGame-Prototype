using UnityEngine;

namespace Singletons
{
    public class SeagullManager : Singleton<SeagullManager>
    {
        [SerializeField, Tooltip("Prefab to create")]
        private GameObject prefab;

        [SerializeField, Min(0), Tooltip("Determines how far the seagulls spawn from the center")]
        private float range = 10;

        private void SpawnSeagull()
        {
            // Spawn seagull
            var seagull = ObjectPools.ObjectPool.GetObject(this.prefab);

            // Randomize
            this.RandomizeSeagull(seagull);
        }

        #region Randomize

        [Header("Randomize")]
        [SerializeField, Min(0), Tooltip("Random offset on the Y axis")]
        private float heightOffset = 5;

        /// <summary>
        /// Randomizes multiple aspects of the seagull
        /// </summary>
        /// <param name="seagull">Seagull to randomize</param>
        private void RandomizeSeagull(GameObject seagull)
        {
            // If no available, skip
            if (seagull == null)
                return;

            // Generate random rotation [0; 360]
            var angle = Random.Range(0, 360) * Mathf.Deg2Rad;

            // Position at random rotation on outer ring
            var offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * this.range;
            seagull.transform.position = this.transform.position + offset;

            // Set random rotation
            seagull.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);

            // Set random height
            seagull.transform.position += new Vector3(0, Random.Range(-this.heightOffset, this.heightOffset), 0);

            // Set random flying speed
            if (seagull.TryGetComponent(out MoveByVector move))
            {
                move.speed = Random.Range(0.5f, 5);
            }

            // Set random turning speed
            if (seagull.TryGetComponent(out RotateByVector rotate))
            {
                rotate.speed = Random.Range(1, 5);
            }

            // Set random TTD time
            if (seagull.TryGetComponent(out ObjectPools.TTD ttd))
            {
                ttd.time = Random.Range(10, 30);
                ttd.ResetSelf();
            }
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start() => this.InvokeRepeating(nameof(SpawnSeagull), 0, 5);

        #endregion

#if UNITY_EDITOR
        #region Gizmos

        /// <inheritdoc/>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(this.transform.position, this.transform.up, this.range);
        }

        #endregion
#endif

        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        #endregion
    }
}