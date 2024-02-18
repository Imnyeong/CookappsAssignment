using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PositionButton))]
public class PositionButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}
