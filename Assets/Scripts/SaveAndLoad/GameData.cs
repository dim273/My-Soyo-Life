using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

[System.Serializable]

public class GameData
{
    public int coin;
    public SerializableDictionary<string, int> inventory;
    public float[] playerStats;
    public string equipedWeapon;
    public bool[] skillLock;
    public List<string> equipedSkill;

    public GameData()
    {
        this.coin = 100;
        inventory = new SerializableDictionary<string, int>();
        playerStats = new float[15];
        equipedWeapon = "empty";
        skillLock = new bool[24];
        equipedSkill = new List<string>();
    }
}
