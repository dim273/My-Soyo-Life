using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    public float CurrentMana {  get; private set; }

    private void Awake()
    {

    }

    private void Start()
    {
        ResetMana();
    }

    private void Update()
    {

    }

    public bool CanRestoreMana()
    {
        return stats.Mana > 0f && stats.Mana < stats.MaxMana;
    }

    public void RestoreMana(float amount)
    {
        stats.Mana += amount;
        stats.Mana = Mathf.Min(stats.Mana, stats.MaxMana);
    }

    public void UseMana(float amount)
    {
        stats.Mana = Mathf.Max(stats.Mana -= amount, 0f);
        CurrentMana = stats.Mana;
    }

    public void ResetMana()
    {
        CurrentMana = stats.MaxMana;
    }
}
