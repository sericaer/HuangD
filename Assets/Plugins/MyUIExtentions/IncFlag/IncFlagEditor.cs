using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(IncFlag))]
public class IncFlagEditor : Editor
{
    public override void OnInspectorGUI()
    {

        var incFlag = target as IncFlag;

        EditorGUI.BeginChangeCheck();

        var isInc = EditorGUILayout.Toggle("isInc", incFlag.isInc);

        if (EditorGUI.EndChangeCheck())
        {
            incFlag.isInc = isInc;
            EditorUtility.SetDirty(incFlag);
        }
    }
}

#endif