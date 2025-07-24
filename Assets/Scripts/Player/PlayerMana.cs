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
        if (Input.GetKeyUp(KeyCode.L))
        {
            UseMana(1f);
        }
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
