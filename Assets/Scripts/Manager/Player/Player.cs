using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    [Header("Test")]
    public HealthPotionItem HealthPotionItem; 
    public ManaPotionItem ManaPotionItem;

    public PlayerStats Stats => stats;

    public PlayerMana PlayerMana { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }

    private PlayerAnimations animations;


    private void Awake()
    {
        animations = GetComponent<PlayerAnimations>();
        PlayerMana = GetComponent<PlayerMana>();
        PlayerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (HealthPotionItem.UseItem())
            {
                Debug.Log("Use Health Potion");
            }
            if (ManaPotionItem.UseItem()) 
            {
                Debug.Log("Use Mana Potion");
            }
        }
    }

    public void ResetPlayer()
    {
        stats.ResetPlayer();
        animations.ResetAnimation();
        PlayerMana.ResetMana();
    }
}
