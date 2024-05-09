using Interfaces;
using UnityEngine;

namespace ObjectPools
{
    /// <summary>
    /// Class that allows objects to despawn naturally
    /// </summary>
    public class TTD : MonoBehaviour, IDespawnable
    {
        [SerializeField, Min(0), Tooltip("Amount of seconds before this GameObject despawns naturally")]
        private float time = 10;
        private float remaining = 0;

        /// <summary>
        /// Updates the time remaining
        /// </summary>
        /// <param name="elapsed">Time passed since the last frame</param>
        private void UpdateTimer(float elapsed)
        {
            // Reduce time
            this.remaining -= elapsed;

            // If time remaining, skip
            if (this.remaining > 0)
                return;

            this.gameObject.SetActive(false);
        }

        #region IDespawnable
        
        /// <inheritdoc/>
        public void ResetSelf() => this.remaining = this.time;

        #endregion

        #region MonoBehaviour

        /// <inheritdoc/>
        private void Update() => this.UpdateTimer(Time.deltaTime);

        #endregion
    }
}