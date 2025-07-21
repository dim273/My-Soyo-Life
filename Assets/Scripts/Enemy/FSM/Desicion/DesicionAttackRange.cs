using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesicionAttackRange : FSMDesicion
{
    [Header("Config")]
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerMask;

    private EnemyBrain enemy;

    private void Awake()
    {
        enemy = GetComponent<EnemyBrain>();
    }
    public override bool Decide()
    {
        return PlayerInAttackRange();
    }

    private bool PlayerInAttackRange()
    {
        if(enemy.player == null) return false;
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackRange, playerMask);
        if(playerCollider == null) return false;
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
