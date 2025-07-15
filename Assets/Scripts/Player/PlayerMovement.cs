using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private float speed;

    private PlayerAC actions;
    private Rigidbody2D rb2D;
    private Player player;
    private PlayerAnimations playerAnimations;

    private Vector2 moveDirection;

    private void Awake()
    {
        actions = new PlayerAC();
        rb2D = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerAnimations = GetComponent<PlayerAnimations>();
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
        if (player.Stats.Health <= 0) return;
        rb2D.MovePosition(rb2D.position + moveDirection * (speed * Time.fixedDeltaTime));
    }

    private void ReadMovement()
    {
        moveDirection = actions.Movement.Move.ReadValue<Vector2>().normalized;
        if (moveDirection == Vector2.zero)
        {
            playerAnimations.SetMoveBool(false);
            return;
        }
        playerAnimations.SetMoveBool(true);
        playerAnimations.SetMoveAnimation(moveDirection);
        
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
