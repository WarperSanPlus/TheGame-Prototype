using UnityEngine;

namespace Singletons
{
    public class OceanManager : Singleton<OceanManager>
    {
        public float strength = 1;
        public float amplitude = 1;

        public static Vector3 GetPosition(Vector3 pos) => Instance.GetPos(pos);

        private Vector3 GetPos(Vector3 pos) 
        {
            pos.y = Mathf.Sin(this.amplitude * (Time.timeSinceLevelLoad + pos.x)) * this.strength;
            return pos;
        }
    }
}