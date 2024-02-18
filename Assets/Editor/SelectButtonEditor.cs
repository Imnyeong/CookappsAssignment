using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SelectButton))]
public class SelectButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}
