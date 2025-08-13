using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class ItemSpawnData
{
    public TileBase Tile;
    public int weight;
}

public class MapGenerator : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private Tilemap itemTileMap;
    [SerializeField] private TileBase grassTile;         // 草地类方块
    [SerializeField] private TileBase desertTile;        // 沙漠类方块
    [SerializeField] private float lacunatity;           // 空隙度
    [SerializeField] private List<ItemSpawnData> spawnDatas; // 装饰物

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
            bool noSeparate = RemoveSeparateTile();
            if (noSeparate) break;
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

        // 计算总的权重
        int weightTotal = 0;
        for (int i = 0; i < spawnDatas.Count; i++)
        {
            weightTotal += spawnDatas[i].weight;
        }

        // 根据权重生成装饰物
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(CheckMapTile(x, y) && GetNeighborGroundCount(x, y) == 8)
                {
                    float randValue = UnityEngine.Random.Range(1, weightTotal + 1);
                    float temp = 0;

                    for(int i = 0; i < spawnDatas.Count; i++)
                    {
                        temp += spawnDatas[i].weight;
                        if(randValue < temp)
                        {
                            if (spawnDatas[i].Tile)
                            {
                                itemTileMap.SetTile(new Vector3Int(x, y), spawnDatas[i].Tile);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    private bool RemoveSeparateTile()
    {
        bool ifSet = false;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(CheckMapTile(x, y) && IsErrowTile(x, y))
                {
                    mapData[x, y] = 1;
                    ifSet = true;
                }
            }
        }
        return ifSet;
    }

    private bool IsErrowTile(int x, int y)
    {
        // 判断该方块是否正确放置
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

    private int GetNeighborGroundCount(int x, int y)
    {
        int count = 0;

        // up
        if (InRange(x, y + 1) && CheckMapTile(x, y + 1)) count++; 
        // down
        if (InRange(x, y - 1) && CheckMapTile(x, y - 1)) count++; 
        // left
        if (InRange(x - 1, y) && CheckMapTile(x - 1, y)) count++; 
        // right
        if (InRange(x + 1, y) && CheckMapTile(x + 1, y)) count++;
        // up left
        if (InRange(x - 1, y + 1) && CheckMapTile(x - 1, y + 1)) count++;
        // up right
        if (InRange(x + 1, y + 1) && CheckMapTile(x + 1, y + 1)) count++;
        // down left
        if (InRange(x - 1, y - 1) && CheckMapTile(x - 1, y - 1)) count++;
        // down right
        if (InRange(x + 1, y - 1) && CheckMapTile(x + 1, y - 1)) count++;

        return count;
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
        itemTileMap.ClearAllTiles();
    }
}
