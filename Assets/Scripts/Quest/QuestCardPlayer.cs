using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestCardPlayer : QuestCard
{
    [Header("Config")]
    [SerializeField] private TextMeshProUGUI statusTMP;
    [SerializeField] private TextMeshProUGUI questRewardTMP;

    public override void ConfigQuestUI(Quest quest)
    {
        base.ConfigQuestUI(quest);
        questRewardTMP.text = $"½±Àø: {quest.GoldReward}½ð±Ò, " +
                              $"{quest.ExpReward}Exp, " +
                              $"x{quest.ItemReward.Quantity}{quest.ItemReward.Item.Name}";
        statusTMP.text = $"½ø¶È\n{quest.CurrentStatus}/{quest.QuestGoal}";
    }
}
