using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

[System.Serializable]

public class GameData
{
    public int coin;
    public SerializableDictionary<string, int> inventory;
    public float[] playerStats;

    public GameData()
    {
        this.coin = 100;
        inventory = new SerializableDictionary<string, int>();
        playerStats = new float[15];
        
    }
}
