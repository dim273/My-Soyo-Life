using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, ISaveManager
{
    [Header("Config")]
    [SerializeField] private WeaponItem initWeapon;         // 初始武器
    [SerializeField] private Transform[] attackPositions;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private GameContent gameContent;

    [Header("Melee Config")]
    [SerializeField] private ParticleSystem slashFX;
    [SerializeField] private float minDistanceMeleeAttack;

    public Weapon curWeapon { get; set; }       // 当前武器

    private bool canAttack;       // 控制攻击的接口

    private WeaponItem curWeaponItme;       // 当前武器对应的武器物品类
    private PlayerAC actions;
    private PlayerAnimations playerAnimations;
    private PlayerMovement playerMovement;
    private PlayerMana playerMana;
    private EnemyBrain enemyTarget;
    private Coroutine attackCoroutine;

    private Transform curAttackPosition;
    private float curAttackRotation;
    private bool equipWeaponInStart = false;

    private void Awake()
    {
        equipWeaponInStart = false;
        actions = new PlayerAC();
        playerAnimations = GetComponent<PlayerAnimations>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMana = GetComponent<PlayerMana>();
    }

    private void Start()
    {
        actions.Attack.ClickAtack.performed += ctx => Attack();
        canAttack = true;
    }

    private void Update()
    {
        GetFireposition();
    }

    private void Attack()
    {
        if(attackCoroutine != null || canAttack == false) return;
        attackCoroutine = StartCoroutine(IEAttack());
    } 

    private void GetFireposition()       // 获取当前的攻击方向
    {
       
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
        if (curAttackPosition == null) curAttackPosition = attackPositions[0];
        playerMovement.ChangeMoveState(false);
        playerAnimations.SetAttackAnimation(true);
        // 选择武器的类别，等待前摇过后启动攻击函数
        yield return new WaitForSeconds(curWeapon.WindUpTime);
        if (curWeapon.type == WeaponType.Magic) 
        {
            if (playerMana.CurrentMana < curWeapon.RequireMana)
            {
                playerMovement.ChangeMoveState(true);
                playerAnimations.SetAttackAnimation(false);
                attackCoroutine = null;
                yield break;
            }
            ManaAttack();
        }
        else
        {
            MeleeAttack();
        }
        yield return new WaitForSeconds(curWeapon.WindDownTime);
        playerAnimations.SetAttackAnimation(false);

        if (curWeapon.type == WeaponType.Magic) playerMovement.ChangeMoveState(true);
        attackCoroutine = null;
    }

    private void ManaAttack()       // 魔法攻击
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, curAttackRotation));
        
        if (enemyTarget != null) 
        {
            // 如果锁定了敌人，则重新计算攻击角度
            Vector2 direction = enemyTarget.transform.position - transform.position;
            float radian =  Mathf.Atan2(direction.y, direction.x);
            float angle =  radian * Mathf.Rad2Deg - 90f;
            rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
        Projectile projectile = Instantiate(curWeapon.ProjectilePrefab, curAttackPosition.position, rotation);
        projectile.Direction = Vector3.up;
        projectile.Damage = GetAttackDamage();
        playerMana.UseMana(curWeapon.RequireMana);
    }

    private void MeleeAttack()      // 物理攻击
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, curAttackRotation));
        Vector2 meleePosition;
        if (curAttackPosition == attackPositions[1] || curAttackPosition == attackPositions[3])
        {
            float _x = transform.position.x;
            float _y = transform.position.y;
            meleePosition = new Vector2(_x, _y - 0.2f);
        }
        else
        {
            meleePosition = transform.position;
        }
        Projectile projectile = Instantiate(curWeapon.ProjectilePrefab, meleePosition, rotation);
        
        projectile.Direction = Vector3.up;
        projectile.Damage = GetAttackDamage();

        //slashFX.transform.position = curAttackPosition.position;
        //slashFX.Play();
        //float currentDistanceToEnemy = Vector3.Distance(enemyTarget.transform.position, transform.position);
        //if(currentDistanceToEnemy <= minDistanceMeleeAttack)
        //{
        //    enemyTarget.GetComponent<IDamageable>().TakeDamage(GetAttackDamage());
        //}
    }

    public void ChangeAttackState(bool value)
    {
        canAttack = value;
    }


    public void EquipWeapon(Weapon newWeapon, WeaponItem weaponItem)        // 换装备
    {
        if (equipWeaponInStart) Inventory.instance.AddItem(curWeaponItme, 1);
        curWeapon = newWeapon;
        curWeaponItme = weaponItem;
    }

    private float GetAttackDamage()
    {
        // 计算伤害，由武器的基础伤害加上角色的基础伤害与武器伤害加成的乘积
        float damage = stats.BaseDamage * curWeapon.DamageBonus + curWeapon.Damage;
        // 计算暴击
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
        if (enemyTarget != null && enemyTarget.enabled) return;
        enemyTarget = null;
    }

    private InventoryItem ItemExistsInGameContent(string itemID)    // 在所有的游戏物品里面寻找对应的物品
    {
        for (int i = 0; i < gameContent.GameItems.Length; i++)
        {
            if (gameContent.GameItems[i].ID == itemID)
            {
                return gameContent.GameItems[i];
            }
        }
        return null;
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

    public void LoadData(GameData _data)
    {
        InventoryItem loadItem = ItemExistsInGameContent(_data.equipedWeapon);
        if (loadItem == null || _data.equipedWeapon == "empty")
        {
            WeaponManager.instance.EquipWeapon(initWeapon.Weapon, initWeapon);
            curWeapon = initWeapon.Weapon;
            curWeaponItme = initWeapon;
        }
        else
        {
            WeaponItem weaponItem = (WeaponItem)loadItem;
            WeaponManager.instance.EquipWeapon(weaponItem.Weapon, weaponItem);
            curWeaponItme = weaponItem;
            curWeapon = weaponItem.Weapon;
        }
        equipWeaponInStart = true;
    }

    public void SaveData(ref GameData _data)
    {
        _data.equipedWeapon = curWeaponItme.ID;
    }
}
