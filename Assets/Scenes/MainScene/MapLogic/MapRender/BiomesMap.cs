using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BiomesMap : MonoBehaviour
{
    [Serializable]
    public struct biomeType2Sprite
    {
        public BiomeType biomeType;
        public Sprite sprite;
    }

    public Tilemap tilemap;

    public biomeType2Sprite[] biomeType2Sprites;

    public void SetCell(Vector3Int vector3Int, BiomeType biomeType)
    {
        //throw new System.NotImplementedException();
    }

    public void SetCell((int x, int y) position, BiomeType biomeType)
    {
        //throw new System.NotImplementedException();
    }
}