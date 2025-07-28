using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Stats")]
public class PlayerStats: ScriptableObject
{
    [Header("Config")]
    public int Level;

    [Header("Health")]
    public float Health;
    public float MaxHealth;

    [Header("Mana")]
    public float Mana;
    public float MaxMana;

    [Header("Exp")]
    public float CurrentExp;
    public float NextLevelExp;
    public float InitNextLevelExp;
    [Range(1f, 100f)] public float ExpMultiplier;

    [Header("Attack")]
    public float BaseDamage;
    public float CriticalChance;
    public float CriticalDamage;

    [HideInInspector] public float TotalExp;
    [HideInInspector] public float TotalDamage;


    public void ResetPlayer()
    {
        Mana = MaxMana;
        Health = MaxHealth;
        CurrentExp = 0f;
        Level = 1;
        NextLevelExp = InitNextLevelExp;
    }
}
