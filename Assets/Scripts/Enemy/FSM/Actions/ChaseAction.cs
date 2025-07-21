using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction : FSMAction
{
    [Header("Config")]
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float attackRange;

    private EnemyBrain enemyBrain;

    private void Awake()
    {
        enemyBrain = GetComponent<EnemyBrain>();
    }

    public override void Act()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (enemyBrain.player == null) return;
        Vector3 dirToPlayer = enemyBrain.player.position - transform.position;
        if(dirToPlayer.magnitude >= attackRange)
        {
            transform.Translate(dirToPlayer.normalized * (chaseSpeed * Time.deltaTime));
        }
    }
}
