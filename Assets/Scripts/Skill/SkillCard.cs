using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : MonoBehaviour
{
    public static event Action<int> OnSkillSelectedEvent;

    [Header("Config")]
    [SerializeField] Skill skill;
    [SerializeField] Sprite lockIcon;
    [SerializeField] Sprite unlockIcon;

    private Image Image;

    public bool ifEquiped {  get; set; }
    public int index {  get; set; }
    public Skill Skill => skill;

    private void Awake()
    {
        Image = GetComponent<Image>();
        Image.sprite = lockIcon;
    }

    public void UnlockSkill()       // 解锁技能
    {
        
    }

    public void Click()     // 点击技能
    {
        OnSkillSelectedEvent?.Invoke(index);
    }

    public void EquipSkillSet()
    {
        ifEquiped = true;
    }

    public void EquipedState(bool value) => ifEquiped = value;
}
