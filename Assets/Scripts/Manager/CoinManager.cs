using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
    [SerializeField] private int TextCoin;
    public int Coins {  get; private set; }
    private const string COIN_KEY = "Coins";

    private void Start()
    {
        Coins = SaveGame.Load(COIN_KEY, TextCoin);
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        SaveGame.Save(COIN_KEY, Coins);
    }

    public void RemoveCoins(int amount)
    {
        if (Coins >= amount) 
        {
            Coins -= amount;
            SaveGame.Save(COIN_KEY, Coins);
        }
    }

}
