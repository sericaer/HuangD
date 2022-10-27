using UnityEngine;

public class MapUIItem : MonoBehaviour
{
    public (int x, int y) cellPos { get; set; }

    internal void SetAlpha(float alpha)
    {
        label.GetComponent<CanvasRenderer>().SetAlpha(alpha);
    }

}