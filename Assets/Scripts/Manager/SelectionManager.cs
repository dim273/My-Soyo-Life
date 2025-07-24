using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    // 创建鼠标选中敌人的事件
    public static event Action<EnemyBrain> OnEnemySelectedEvent;
    public static event Action OnNoSelectedEvent;

    [Header("Config")]
    [SerializeField] private LayerMask enemyMask;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        SelectEnemy();
    }

    private void SelectEnemy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, enemyMask);
            if (hit.collider != null)
            {
                EnemyBrain enemy = hit.collider.GetComponent<EnemyBrain>();
                if (enemy != null)
                {
                    OnEnemySelectedEvent?.Invoke(enemy);
                }
            }
            else
            {
                OnNoSelectedEvent?.Invoke();
            }
        }
    }


}
