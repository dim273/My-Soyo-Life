using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    public PlayerStats Stats => stats;

    public PlayerMana PlayerMana { get; private set; }

    private PlayerAnimations animations;


    private void Awake()
    {
        animations = GetComponent<PlayerAnimations>();
        PlayerMana = GetComponent<PlayerMana>();
    }

    public void ResetPlayer()
    {
        stats.ResetPlayer();
        animations.ResetAnimation();
        PlayerMana.ResetMana();
    }
}
