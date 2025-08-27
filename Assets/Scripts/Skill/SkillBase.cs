using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] SkillType skillType;

    protected virtual void Start()
    {
        if (skillType == SkillType.Passive) UseSkill();
    }

    public virtual void UseSkill()
    {

    }
}
