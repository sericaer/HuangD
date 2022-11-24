#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public static class IncFlagResigter
{
    [MenuItem("GameObject/UI/MyUIExtentions/IncFlagPanel")]
    public static void AddDialogPanel(MenuCommand menuCommand)
    {
        var parent = menuCommand.context as GameObject;
        var instance = Object.Instantiate(Resources.Load("IncFlag"), parent.transform) as GameObject;
        instance.name = "IncFlag";
    }
}

#endif