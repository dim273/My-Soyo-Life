using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAC actions;
    private PlayerAnimations playerAnimations;
    private EnemyBrain enemyTarget;
    private Coroutine attackCoroutine;

    private void Awake()
    {
        actions = new PlayerAC();
        playerAnimations = GetComponent<PlayerAnimations>();
    }

    private void Start()
    {
        actions.Attack.ClickAtack.performed += ctx => Attack();
    }

    private void Attack()
    {
        if (enemyTarget == null) return;
        if(attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }

        attackCoroutine = StartCoroutine(IEAttack());
    } 

    private IEnumerator IEAttack()
    {
        playerAnimations.SetAttackAnimation(true);
        yield return new WaitForSeconds(0.5f);
        playerAnimations.SetAttackAnimation(false);
    }

    private void EnemySelectedCallback(EnemyBrain enemySelected)
    {
        enemyTarget = enemySelected;

    }

    private void NoEnemySelectedCallback()
    {
        enemyTarget = null;
    }

    private void OnEnable()
    {
        actions.Enable();
        SelectionManager.OnEnemySelectedEvent += EnemySelectedCallback;
        SelectionManager.OnNoSelectedEvent += NoEnemySelectedCallback;
    }

    private void OnDisable()
    {
        actions.Disable();
        SelectionManager.OnEnemySelectedEvent -= EnemySelectedCallback;
        SelectionManager.OnNoSelectedEvent -= NoEnemySelectedCallback;
    }
}
