using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BehaviourTree.Tree), true)]
public class BehaviourTreeEditor : Editor 
{
    void OnEnable()
    {
        //lookAtPoint = serializedObject.FindProperty("lookAtPoint");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var target = (BehaviourTree.Tree)this.target;

        if (GUILayout.Button("Refresh"))
        {
            if (Application.isPlaying)
                Debug.Log($"Refresh '{target.gameObject.name}'");
            target.RefreshTree();
        }
    }
}