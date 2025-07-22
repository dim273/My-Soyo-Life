using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : FSMAction
{
    [Header("Config")]
    [SerializeField] private float damage;
    [SerializeField] private float timeBtwAttack;

    private EnemyBrain enemyBrain;
    private float timer;

    private void Awake()
    {
        enemyBrain = GetComponent<EnemyBrain>();
        timer = timeBtwAttack;
    }

    public override void Act()
    {
        AttackPlayer();
    }

    private void AttackPlayer()
    {
        if (enemyBrain.player == null)  return; 
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            IDamageable player = enemyBrain.player.GetComponent<IDamageable>();
            player.TakeDamage(damage);
            //PlayerHealth playerHealth = enemyBrain.player.GetComponent<PlayerHealth>();
            //playerHealth.TakeDamage(damage);
            timer = timeBtwAttack;
        }
    }
}
