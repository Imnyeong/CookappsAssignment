using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillButton))]
public class SkillButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}
