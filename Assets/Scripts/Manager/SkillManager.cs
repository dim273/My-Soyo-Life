using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>, ISaveManager
{
    [Header("Config")]
    [SerializeField] private GameContent GameContent;
    [SerializeField] private GameObject[] skillsCards;
    [SerializeField] private Transform activeSkillContainer;
    [SerializeField] private Transform passiveSkillContainer;

    [Header("Desciption Panel")]
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;



    public GameObject selectedSkill {  get; set; }

    private List<Skill> equipedActiveSkill;
    private List<Skill> equipedPassiveSkill;

    // 保存数据用
    private bool[] skillLockData = new bool[24];        // 用来储存技能的解锁情况
    


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
            SkillCard _skillCard = skillsCards[i].GetComponent<SkillCard>();
            if (skillLockData[i]) _skillCard.UnlockSkill();
            _skillCard.index = i;
        }

    }

    public void EquipSkill(Skill _skill)        // 装备技能
    {
        if (_skill == null) return;
        
        if (_skill.Type == SkillType.Passive)
        {
            if (equipedPassiveSkill.Count >= 2) return;
            equipedPassiveSkill.Add(_skill);
            GameObject _skillCard = Instantiate(_skill.skillCard,passiveSkillContainer);
            _skillCard.GetComponent<SkillCard>().ifEquiped = true;
            _skill.skillCard.GetComponent<SkillCard>().ifEquiped = true;
        }
        else
        {
            if (equipedActiveSkill.Count >= 2) return;
            equipedActiveSkill.Add(_skill);
            GameObject _skillCard = Instantiate(_skill.skillCard, activeSkillContainer);
            _skillCard.GetComponent<SkillCard>().ifEquiped = true;
            _skill.skillCard.GetComponent<SkillCard>().ifEquiped = true;
        }

    }

    public void RemoveSkill(Skill _skill)       // 卸载技能
    {
        if (_skill == null) return;
        _skill.skillCard.GetComponent<SkillCard>().ifEquiped = false;
        if (_skill.Type == SkillType.Passive)
        {
            equipedPassiveSkill.RemoveAll(it => it.ID == _skill.ID);
        }
        else
        {
            equipedActiveSkill.RemoveAll(it => it.ID == _skill.ID);
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

    // 各类按钮区
    /*----------------------------------------------------------------------------*/
    public void EquipButton()       // 装备按钮
    {
        if (selectedSkill == null || selectedSkill.GetComponent<SkillCard>().ifEquiped) return;
        EquipSkill(selectedSkill.GetComponent<SkillCard>().Skill);
    }

    public void RemoveButton()      // 卸下按钮
    {
        if (selectedSkill == null || !selectedSkill.GetComponent<SkillCard>().ifEquiped) return;
        RemoveSkill(selectedSkill.GetComponent<SkillCard>().Skill);
    }

    public void ClearAllButton()        // 清除全部按钮
    {
        foreach (Skill _skill in equipedActiveSkill)
            RemoveSkill(_skill);
        foreach (Skill _skill in equipedPassiveSkill)
            RemoveSkill(_skill);
    }
    /*----------------------------------------------------------------------------*/


    private void ShowSkillInfo(int index)
    {
        title.text = GameContent.Skills[index].Name;
        description.text = GameContent.Skills[index].Description;   
        selectedSkill = skillsCards[index];
    }

    private void OnEnable()
    {
        SkillCard.OnSkillSelectedEvent += ShowSkillInfo;
    }

    private void OnDisable()
    {
        SkillCard.OnSkillSelectedEvent -= ShowSkillInfo;
    }

    public void LoadData(GameData _data)
    {
        for (int i = 0; i < 24; i++)
        {
            skillLockData[i] = _data.skillLock[i];
        }
        foreach (string key in _data.equipedSkill)
        {
            Skill _skill = FetchSkillEquiped(key);
            EquipSkill(_skill);
        }
    }

    public void SaveData(ref GameData _data)
    {
        for (int i = 0; i < 24; i++)
        {
            _data.skillLock[i] = skillLockData[i];
        }

        foreach (Skill _skill in equipedActiveSkill)
            _data.equipedSkill.Add(_skill.ID);
        foreach (Skill _skill in equipedPassiveSkill)
            _data.equipedSkill.Add(_skill.ID);
        
    }
}

