using System.Collections.Generic;
using UnityEngine;

namespace ObjectPools
{
    /// <summary>
    /// Class that manages the reutilisation of GameObject
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField, Tooltip("GameObject to instantiate upon creation")]
        private GameObject prefab;

        [SerializeField, Min(1), Tooltip("Amount of GameObject to create upon creation")]
        private int preAmount = 10;

        [SerializeField, Min(1), Tooltip("Maximum amount of GameObject this pool can create")]
        private int hardCap = int.MaxValue;

        /// <summary>
        /// Spawns an instance of the prefab
        /// </summary>
        /// <returns>Spawned instance</returns>
        /// <remarks>
        /// This method returns null if the hard cap has been reached
        /// </remarks>
        public GameObject Spawn()
        {
            // Skip if hard cap reached
            if (this.transform.childCount >= this.hardCap)
                return null;

            var newObject = GameObject.Instantiate(this.prefab, this.transform);
            newObject.SetActive(false);

            return newObject;
        }

        /// <returns>First instance available</returns>
        private GameObject GetInstance()
        {
            GameObject instance = null;

            // Find the first inactive child
            for (var i = 0; i < this.transform.childCount; i++)
            {
                var child = this.transform.GetChild(i).gameObject;

                // If child active, skip
                if (child.activeInHierarchy)
                    continue;

                instance = child;
                break;
            }

            // If not found, create
            if (instance != null)
                return instance;

#if UNITY_EDITOR
            Debug.LogWarning($"Consider increasing the base amount of '{this.prefab.name}' if the limit is reached frequently.");
#endif
            return this.Spawn();
        }

        /// <summary>
        /// Finds a registered pool for the given prefab
        /// </summary>
        /// <returns>Found pool</returns>
        /// <remarks>
        /// This method returns null if no pool has been registered with this prefab
        /// </remarks>
        public static ObjectPool GetPool(GameObject prefab)
        {
            if (prefab == null)
                return null;

            var name = prefab.name;

            return registeredPools.TryGetValue(name, out var pool) && pool == null ? null : pool;
        }

        /// <summary>
        /// Finds an available instance of the given prefab and returns it
        /// </summary>
        /// <returns>Found instance</returns>
        /// <remarks>
        /// This method returns null if the hard cap has been reached
        /// </remarks>
        public static GameObject GetObject(GameObject prefab)
        {
            var pool = GetPool(prefab);

            // If no pool found
            if (pool == null)
                return null;

            var instance = pool.GetInstance();

            // Initialize instance
            if (instance != null)
            {
                instance.SetActive(true);

                // Reset all IDespawnable
                foreach (var item in instance.GetComponents<Interfaces.IDespawnable>())
                    item.ResetSelf();
            }

            return instance;
        }

        #region Register

        /// <summary>
        /// List of pools registered
        /// </summary>
        private static readonly Dictionary<string, ObjectPool> registeredPools = new();

        /// <summary>
        /// Tries to register itself
        /// </summary>
        private void RegisterSelf()
        {
            // Skip if prefab invalid
            if (this.prefab == null)
                return;

            var name = this.prefab.name;

            // If a pool already exists for this, skip
            if (registeredPools.TryGetValue(name, out var pool) && pool != null)
            {
                Debug.LogWarning($"{this.gameObject.name} can't be created, because another object pool already exists for the prefab '{name}'");
                return;
            }

            // Initialize all instances
            for (var i = 0; i < this.preAmount; i++)
                _ = this.Spawn();

            registeredPools[name] = this;
        }

        /// <summary>
        /// Removes itself from the registered pools
        /// </summary>
        private void PurgeSelf()
        {
            // Skip if prefab invalid
            if (this.prefab == null)
                return;

            var name = this.prefab.name;

            // If not registered or not the actual pool, skip
            if (!registeredPools.TryGetValue(name, out var pool) || pool != this)
                return;

            // Remove itself from registered
            _ = registeredPools.Remove(name);
        }

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Start() => this.RegisterSelf();

        /// <inheritdoc/>
        private void OnDestroy() => this.PurgeSelf();

        #endregion
    }
}