#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class PieChartResigter
{
    [MenuItem("GameObject/UI/MyUIExtentions/PieChartExt")]
    public static void AddDialogPanel(MenuCommand menuCommand)
    {
        var parent = menuCommand.context as GameObject;
        var instance = Object.Instantiate(Resources.Load("PieChartExt"), parent.transform) as GameObject;
        instance.name = "PieChartExt";
    }
}

#endif
