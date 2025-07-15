using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    private void Awake()
    {

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
        if(stats.Mana >= amount)
        {
            stats.Mana = Mathf.Max(stats.Mana -= amount, 0f);
        }
    }

}
