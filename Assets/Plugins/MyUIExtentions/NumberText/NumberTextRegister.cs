#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class NumberTextRegister
{
    [MenuItem("GameObject/UI/MyUIExtentions/NumberText")]
    public static void AddDialogPanel(MenuCommand menuCommand)
    {
        var parent = menuCommand.context as GameObject;
        var instance = Object.Instantiate(Resources.Load("NumberText"), parent.transform) as GameObject;
        instance.name = nameof(NumberText);
    }
}

#endif