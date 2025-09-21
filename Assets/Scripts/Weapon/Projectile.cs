using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float speed;
    [SerializeField] private float destroyTime;

    public Vector3 Direction { get; set; }
    public float Damage {  get; set; }

    private float timer;

    private void Start()
    {
        timer = destroyTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0) Destroy(gameObject); 
        transform.Translate(Direction * (speed * Time.deltaTime));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<IDamageable>()?.TakeDamage(Damage);
        if (speed != 0) DestroyProjectile();
    }

    public void DestroyProjectile() => Destroy(gameObject);

    public void StopAttack()
    {
        GameManager.instance.PlayerAttack.ChangeAttackState(false);
        GameManager.instance.PlayerMovement.ChangeMoveState(false);
    }
    public void RecoverAttack()
    {
        GameManager.instance.PlayerAttack.ChangeAttackState(true);
        GameManager.instance.PlayerMovement.ChangeMoveState(true);
    }

}
