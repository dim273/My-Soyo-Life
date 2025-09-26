using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBlessSkill : SkillBase
{
    [Header("SkillInfo")]
    [SerializeField] private float speedAdd;

    public float SpeedAdd { set; get; }

    public override void UseSkill()
    {
        GameManager.instance.Player.PlayerMovement.ChangeMoveSpeed(SpeedAdd);
    }
}
