using UnityEngine;

public class Ocean : MonoBehaviour
{
    public static Vector3 GetPosition(Vector3 pos) {

        pos.y = Mathf.Sin(Time.timeSinceLevelLoad + pos.x);
        return pos;
    }
}
