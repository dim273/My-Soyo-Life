using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Player Info")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;

    public Player Player => player;
    public PlayerAttack PlayerAttack => playerAttack;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            player.ResetPlayer();
        }
    }

    public void AddPlayerExp(float amount)
    {
        PlayerExp playerExp = player.GetComponent<PlayerExp>();
        playerExp.AddExp(amount);
    }
}

