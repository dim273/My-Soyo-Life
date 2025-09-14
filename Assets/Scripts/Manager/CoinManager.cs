using UnityEngine;

public class CoinManager : Singleton<CoinManager>, ISaveManager
{
    [SerializeField] private int TextCoin;
    public int Coins {  get; private set; }

    private void Start()
    {
        
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
    }

    public void RemoveCoins(int amount)
    {
        if (Coins >= amount) 
        {
            Coins -= amount;
        }
    }

    public void LoadData(GameData _data)
    {
        Coins = _data.coin;
    }

    public void SaveData(ref GameData _data)
    {
        _data.coin = Coins;
    }
}
