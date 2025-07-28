using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Weapon initWeapon;
    [SerializeField] private Transform[] attackPositions;
    [SerializeField] private PlayerStats stats;

    [Header("Melee Config")]
    [SerializeField] private ParticleSystem slashFX;
    [SerializeField] private float minDistanceMeleeAttack;

    public Weapon curWeapon { get; set; }

    private PlayerAC actions;
    private PlayerAnimations playerAnimations;
    private PlayerMovement playerMovement;
    private PlayerMana playerMana;
    private EnemyBrain enemyTarget;
    private Coroutine attackCoroutine;

    private Transform curAttackPosition;
    private float curAttackRotation;

    private void Awake()
    {
        actions = new PlayerAC();
        playerAnimations = GetComponent<PlayerAnimations>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMana = GetComponent<PlayerMana>();
    }

    private void Start()
    {
        EquipWeapon(initWeapon);
        actions.Attack.ClickAtack.performed += ctx => Attack();
    }

    private void Update()
    {
        GetFireposition();
    }

    private void Attack()
    {
        if (enemyTarget == null) return;
        if(attackCoroutine != null)
        {
            // 如果存在进程，则停止
            StopCoroutine(attackCoroutine);
        }

        attackCoroutine = StartCoroutine(IEAttack());
    } 

    private void GetFireposition()
    {
        // 获取当前的攻击方向
        Vector2 moveDirection = playerMovement.MoveDirection;
        switch (moveDirection.x)
        {
            case > 0f:
                curAttackPosition = attackPositions[1];
                curAttackRotation = -90f;
                break;
            case < 0f:
                curAttackPosition = attackPositions[3];
                curAttackRotation = -270f;
                break;
        }

        switch (moveDirection.y)
        {
            case > 0f:
                curAttackPosition = attackPositions[0];
                curAttackRotation = 0f;
                break;
            case < 0f:
                curAttackPosition = attackPositions[2];
                curAttackRotation = -180f;
                break;
        }
    }

    private IEnumerator IEAttack()
    {
        // 如果位置没有，则返回
        if (curAttackPosition == null) yield break;
        // 选择武器的类别
        if (curWeapon.type == WeaponType.Magic) 
        {
            if (playerMana.CurrentMana < curWeapon.RequireMana) yield break;
            ManaAttack();
        }
        else
        {
            MeleeAttack();
        }
        playerAnimations.SetAttackAnimation(true);
        yield return new WaitForSeconds(0.5f);
        playerAnimations.SetAttackAnimation(false);

        
    }

    private void ManaAttack()
    {
        // 魔法攻击
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, curAttackRotation));
        Projectile projectile = Instantiate(curWeapon.ProjectilePrefab, curAttackPosition.position, rotation);
        projectile.Direction = Vector3.up;
        projectile.Damage = GetAttackDamage();
        playerMana.UseMana(curWeapon.RequireMana);
    }

    private void MeleeAttack()
    {
        // 物理攻击
        slashFX.transform.position = curAttackPosition.position;
        slashFX.Play();
        float currentDistanceToEnemy = Vector3.Distance(enemyTarget.transform.position, transform.position);
        if(currentDistanceToEnemy <= minDistanceMeleeAttack)
        {
            enemyTarget.GetComponent<IDamageable>().TakeDamage(GetAttackDamage());
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        curWeapon = newWeapon;
        stats.TotalDamage = stats.BaseDamage + curWeapon.Damage;
    }

    private float GetAttackDamage()
    {
        float damage = stats.BaseDamage;
        damage += curWeapon.Damage;
        float randomPerc = Random.Range(0f, 100);
        if(randomPerc <= stats.CriticalChance)
        {
            damage += damage * (stats.CriticalDamage / 100f);
        }
        return damage;
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
        EnemyHealth.OnEnemyDeadEvent += NoEnemySelectedCallback;
    }

    private void OnDisable()
    {
        actions.Disable();
        SelectionManager.OnEnemySelectedEvent -= EnemySelectedCallback;
        SelectionManager.OnNoSelectedEvent -= NoEnemySelectedCallback;
        EnemyHealth.OnEnemyDeadEvent -= NoEnemySelectedCallback;
    }
}
