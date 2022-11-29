#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class DialogPanelResigter
{
    [MenuItem("GameObject/UI/MyUIExtentions/DialogPanel")]
    public static void AddDialogPanel(MenuCommand menuCommand)
    {
        var parent = menuCommand.context as GameObject;
        var instance = Object.Instantiate(Resources.Load("DialogPanel"), parent.transform) as GameObject;
        instance.name = "DialogPanel";
    }
}

#endif