using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCarveMap : MapBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void SetCell(Vector3Int vector3Int, float value)
    {
        tilemap.SetTileColor(vector3Int, tile, new Color(value, value, value));
    }
}
