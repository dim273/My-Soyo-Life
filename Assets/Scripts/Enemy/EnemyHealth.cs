using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public static event Action OnEnemyDeadEvent;

    [Header("Config")]
    [SerializeField] private float health;

    public float CurrentHealth { get; private set; }

    private Animator animator;
    private EnemyBrain enemyBrain;
    private EnemySelector enemySelector;
    private EnemyLoot enemyLoot;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyBrain = GetComponent<EnemyBrain>();
        enemySelector = GetComponent<EnemySelector>();
        enemyLoot = GetComponent<EnemyLoot>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        CurrentHealth = health;
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        if(CurrentHealth <= 0)
        {
            EnemyDead();

            // É±ËÀµÐÈËµÄÈÎÎñ
            QuestManager.instance.AddProgress("Kill10Enemy", 1);
        }
        else
        {
            DamageManager.instance.ShowDamageText(amount, transform);
        }
    }

    private void EnemyDead()
    {
        // µÐÈËËÀÍö£¬²¥·ÅËÀÍö¶¯»­£¬¹Ø±Õai£¬ÉèÖÃÅö×²£¬µôÂä
        animator.SetTrigger("Dead");
        enemyBrain.enabled = false;

        if (enemySelector.ReturnStateOfSprite()) enemySelector.NoSelectionCallback();

        rb2D.bodyType = RigidbodyType2D.Static;
        OnEnemyDeadEvent?.Invoke();
        GameManager.instance.AddPlayerExp(enemyLoot.ExpDrop);
    }
}
