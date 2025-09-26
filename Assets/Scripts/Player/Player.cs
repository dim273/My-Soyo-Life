using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ISaveManager
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    public PlayerStats Stats => stats;

    public PlayerMana PlayerMana { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerAttack PlayerAttack { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }

    private PlayerAnimations animations;


    private void Awake()
    {
        animations = GetComponent<PlayerAnimations>();
        PlayerMana = GetComponent<PlayerMana>();
        PlayerHealth = GetComponent<PlayerHealth>();
        PlayerAttack = GetComponent<PlayerAttack>();
        PlayerMovement = GetComponent<PlayerMovement>();
    }


    public void ResetPlayer()
    {
        stats.ResetPlayer();
        animations.ResetAnimation();
        PlayerMana.ResetMana();
    }

    public void LoadData(GameData _data)
    {
        if (_data.playerStats[0] < 1)
        {
            // 新游戏时防止读取存档中的空数据
            stats.ResetPlayer();
            return;
        }
        stats.Level = (int)_data.playerStats[0];
        stats.Health = _data.playerStats[1];
        stats.MaxHealth = _data.playerStats[2];
        stats.Mana = _data.playerStats[3];
        stats.MaxMana = _data.playerStats[4];
        stats.CurrentExp = _data.playerStats[5];
        stats.NextLevelExp = _data.playerStats[6];
        stats.BaseDamage = _data.playerStats[7];
        stats.CriticalChance = _data.playerStats[8];
        stats.CriticalDamage = _data.playerStats[9];
        stats.Strength = (int)_data.playerStats[10];
        stats.Dexterity = (int)_data.playerStats[11];
        stats.Intelligence = (int)_data.playerStats[12];
        stats.AttributePoints = (int)_data.playerStats[13];
        stats.TotalExp = _data.playerStats[14];
    }

    public void SaveData(ref GameData _data)
    {
        _data.playerStats[0] = stats.Level;
        _data.playerStats[1] = stats.Health;
        _data.playerStats[2] = stats.MaxHealth;
        _data.playerStats[3] = stats.Mana;
        _data.playerStats[4] = stats.MaxMana;
        _data.playerStats[5] = stats.CurrentExp;
        _data.playerStats[6] = stats.NextLevelExp;
        _data.playerStats[7] = stats.BaseDamage;
        _data.playerStats[8] = stats.CriticalChance;
        _data.playerStats[9] = stats.CriticalDamage;
        _data.playerStats[10] = stats.Strength;
        _data.playerStats[11] = stats.Dexterity;
        _data.playerStats[12] = stats.Intelligence;
        _data.playerStats[13] = stats.AttributePoints;
        _data.playerStats[14] = stats.TotalExp;
    }
}
