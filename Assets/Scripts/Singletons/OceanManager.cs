
using UnityEngine;

namespace Singletons
{
    /// <summary>
    /// Class that manages how waves behave
    /// </summary>
    public class OceanManager : Singleton<OceanManager> 
    {
        [SerializeField, Tooltip("Material used to display the waves")]
        private Material waveMateral;
        
        /// <param name="position">Original position</param>
        /// <returns>Y position of the wave</returns>
        public static float GetHeight(Vector3 position, float offset = 0)
        {
            var material = Instance.waveMateral;

            if (material == null)
                return position.y;

            var frequency = material.GetFloat("_Frequency");
            var amplitude = material.GetFloat("_Amplitude");

            var x = position.z + Time.timeSinceLevelLoad;

            return (Mathf.Sin(x * frequency) * amplitude) + offset;
        }

        #region Singleton

        /// <inheritdoc/>
        protected override bool DestroyOnLoad => true;

        #endregion
    }
}