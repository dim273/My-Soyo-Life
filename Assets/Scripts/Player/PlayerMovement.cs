using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header(" Ù–‘")]
    [SerializeField] private float speed;

    private PlayerAC actions;
    private Rigidbody2D rb2D;

    private Vector2 moveDirection;

    private void Awake()
    {
        actions = new PlayerAC();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        ReadMovement();
    }

    private void Move()
    {
        rb2D.MovePosition(rb2D.position + moveDirection * (speed * Time.fixedDeltaTime));
    }

    private void ReadMovement()
    {
        moveDirection = actions.Movement.Move.ReadValue<Vector2>().normalized;
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }
}
