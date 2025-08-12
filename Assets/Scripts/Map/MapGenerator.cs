using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private TileBase grassTile;         // 草地类方块
    [SerializeField] private TileBase desertTile;        // 沙漠类方块
    [SerializeField] private float lacunatity;           // 空隙度

    [Header("Map")]
    public int width;                   // 宽
    public int height;                  // 高
    [Range(0, 1f)]
    public float desertProbability;     // 沙漠的比例

    [Header("Seed")]
    [SerializeField] private int seed;                   // 种子
    [SerializeField] private bool useRandomSeed;         // 是否使用种子

    [Header("Repair")]
    [SerializeField] private int removeSeparateTileNumberOfTimes;

    private float[,] mapData;           // 地图的信息


    public void GenerateMap()
    {
        GenerateMapData();
        for (int i = 0; i < removeSeparateTileNumberOfTimes; i++) 
        {
            RemoveSeparateTile();
        }
        GenerateTileMap();
    }

    private void GenerateMapData()
    {
        // 对于种子的处理
        if (!useRandomSeed) seed = Time.time.GetHashCode();
        UnityEngine.Random.InitState(seed);

        mapData = new float[width, height];

        float randomOffset = UnityEngine.Random.Range(-10000, 10000);

        float maxValue = float.MinValue;
        float minValue = float.MaxValue;

        for (int x = 0; x < width; x++) 
        {
            for(int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise(x * lacunatity + randomOffset, y * lacunatity + randomOffset);
                mapData[x, y] = noiseValue;
                if (noiseValue < minValue) minValue = noiseValue;
                if (noiseValue > maxValue) maxValue = noiseValue;
            }
        }

        // 平滑到0~1
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = Mathf.InverseLerp(minValue, maxValue, mapData[x, y]);
            }
        }
    }

    private void GenerateTileMap()
    {
        CleanMap();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileBase tile = CheckMapTile(x, y) ? grassTile : desertTile;
                groundTileMap.SetTile(new Vector3Int(x, y), tile);
            }
        }
    }

    private void RemoveSeparateTile()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(CheckMapTile(x, y) && GetNeighborGroundCount(x, y))
                {
                    mapData[x, y] = 1;
                }
            }
        }
    }

    private bool GetNeighborGroundCount(int x, int y)
    {
        int count = 0;
        bool _up = false, _down = false, _left = false, _right = false;
        // up
        if (InRange(x, y + 1) && CheckMapTile(x, y + 1)) { _up = true; count++; }
        // down
        if (InRange(x, y - 1) && CheckMapTile(x, y - 1)) { _down = true; count++; }
        // left
        if (InRange(x - 1, y) && CheckMapTile(x - 1, y)) { _left = true; count++; }
        // right
        if (InRange(x + 1, y) && CheckMapTile(x + 1, y)) { _right = true; count++; }

        // 如果只有一个相邻的块或者两种特殊情况，直接返回true
        if (count <= 1) return true;
        if (_right && _left && !_up && !_down) return true;
        if (!_right && !_left && _up && _down) return true;
        return false;
    }

    private bool InRange(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    private bool CheckMapTile(int x, int y)
    {
        return mapData[x, y] < desertProbability;
    }

    public void CleanMap()
    {
        groundTileMap.ClearAllTiles();
    }
}
