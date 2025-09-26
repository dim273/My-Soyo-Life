using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    Active,
    Passive
}

[CreateAssetMenu]
public class Skill : ScriptableObject
{
    [Header("Config")]
    public string Name;
    public string ID;
    public SkillBase skillBase;
    public SkillType Type;

    [Header("Info")]
    [TextArea] public string Description;
}

