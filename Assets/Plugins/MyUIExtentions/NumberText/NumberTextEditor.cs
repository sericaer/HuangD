#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NumberText))]
public class NumberTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        var numberText = target as NumberText;

        EditorGUI.BeginChangeCheck();

        var decimals = EditorGUILayout.IntField("decimals", numberText.decimals);

        if (EditorGUI.EndChangeCheck())
        {
            numberText.decimals = decimals;
            EditorUtility.SetDirty(numberText);
        }

        EditorGUI.BeginChangeCheck();

        var showSign = EditorGUILayout.Toggle("showSign", numberText.showSign);

        if (EditorGUI.EndChangeCheck())
        {
            numberText.showSign = showSign;
            EditorUtility.SetDirty(numberText);
        }

        EditorGUI.BeginChangeCheck();

        var PositiveColor = EditorGUILayout.ColorField("+ color", numberText.PositiveColor);

        if (EditorGUI.EndChangeCheck())
        {
            numberText.PositiveColor = PositiveColor;
            EditorUtility.SetDirty(numberText);
        }

        EditorGUI.BeginChangeCheck();

        var NegativeColor = EditorGUILayout.ColorField("- color", numberText.NegativeColor);

        if (EditorGUI.EndChangeCheck())
        {
            numberText.NegativeColor = NegativeColor;
            EditorUtility.SetDirty(numberText);
        }

        EditorGUI.BeginChangeCheck();

        var Value = EditorGUILayout.DoubleField("Value", numberText.Value);

        if (EditorGUI.EndChangeCheck())
        {
            numberText.Value = Value;
            EditorUtility.SetDirty(numberText);
        }

        var Format = EditorGUILayout.TextField("Format", numberText.Format);

        if (EditorGUI.EndChangeCheck())
        {
            numberText.Format = Format;
            EditorUtility.SetDirty(numberText);
        }
    }
}
#endif