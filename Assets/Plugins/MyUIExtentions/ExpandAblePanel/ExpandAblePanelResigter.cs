#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ExpandAblePanelResigter
{
    [MenuItem("GameObject/UI/MyUIExtentions/ExpandAblePanel")]
    public static void AddDialogPanel(MenuCommand menuCommand)
    {
        var parent = menuCommand.context as GameObject;
        var instance = Object.Instantiate(Resources.Load("ExpandAblePanel"), parent.transform) as GameObject;
        instance.name = "ExpandAblePanel";
    }
}

#endif