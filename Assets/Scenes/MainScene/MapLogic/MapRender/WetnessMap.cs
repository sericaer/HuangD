using UnityEngine;

public class WetnessMap : MapBehaviour
{
    public Color WetMax = Color.green;
    public Color WetMin = Color.yellow;

    internal void SetCell(Vector3Int vector3Int, float value)
    {
        var color = CalcColor(value);

        tilemap.SetTileColor(vector3Int, tile, color);
    }

    internal void SetCell((int x, int y) position, float value)
    {
        var color = CalcColor(value);

        tilemap.SetTileColor(new Vector3Int(position.x, position.y), tile, color);
    }

    private Color CalcColor(float value)
    {
        return WetMin + (WetMax - WetMin) * value;
    }
}