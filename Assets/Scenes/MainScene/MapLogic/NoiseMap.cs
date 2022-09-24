using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NoiseMap : MapBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCell((int x, int y) pos, float value)
    {
        tilemap.SetTileColor(new Vector3Int(pos.x, pos.y), tile, new Color(value, value, value));
    }
}
