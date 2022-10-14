using UnityEngine;

public class PopulationMap : MapBehaviour
{
    public Color rainMax = Color.red;
    public Color rainMin = Color.white;

    internal void SetCell(Vector3Int vector3Int, int Value)
    {
        var color = CalcColor(Value / 10000f);

        tilemap.SetTileColor(vector3Int, tile, color);
    }

    private Color CalcColor(float value)
    {
        return rainMin + (rainMax - rainMin) * value;
    }
}