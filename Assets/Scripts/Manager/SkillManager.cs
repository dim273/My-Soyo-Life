using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [Header("Config")]
    [SerializeField] private GameContent GameContent;
    [SerializeField] private Transform[] skillsCards;
    [SerializeField] private Transform activeSkillContainer;
    [SerializeField] private Transform passiveSkillContainer;



    private Skill[] equipedActiveSkill;
    private Skill[] equipedPassiveSkill;
    private bool[] skillLockData = new bool[24];
    private string[] skillEquipedID = new string[4];
    


    private void Start()
    {
        InitSkillsPanel();
        PassiveSkillUse();
        
    }

    private void Update()
    {
        ActiveSkillUse();
    }

    private void InitSkillsPanel()          // 初始化技能列表
    {
        for (int i = 0; i < 24; i ++)
        {
            if (skillLockData[i]) skillsCards[i].GetComponent<SkillCard>().UnlockSkill();
        }

    }

    public void EquipSkill(Skill _skill)
    {
        if (_skill == null) return;
        
        if (_skill.Type == SkillType.Passive)
        {
            if (equipedPassiveSkill.Length >= 2) return;
        }
        else
        {
            if (equipedActiveSkill.Length >= 2) return;
        }

    }

    private Skill FetchSkillEquiped(string _ID)       // 遍历所有技能寻找装配的技能
    {
        for (int i = 0; i < GameContent.Skills.Length; i++)
        {
            if (GameContent.Skills[i].ID == _ID)
            {
                return GameContent.Skills[i];
            }
        }
        return null;
    }

    private void PassiveSkillUse()      // 使用已装备的被动技能
    {
        
    }

    private void ActiveSkillUse()       // 使用已装备的主动技能
    {

    }

}

