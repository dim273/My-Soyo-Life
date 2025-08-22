using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestCardPlayer : QuestCard
{
    [Header("Config")]
    [SerializeField] private TextMeshProUGUI statusTMP;
    [SerializeField] private TextMeshProUGUI questRewardTMP;

    [Header("Claim")]
    [SerializeField] private GameObject claimButton;
    [SerializeField] private GameObject questStatus;

    private void Update()
    {
        statusTMP.text = $"进度\n{QuestToComplete.CurrentStatus}/{QuestToComplete.QuestGoal}";
    }

    public override void ConfigQuestUI(Quest quest)
    {
        base.ConfigQuestUI(quest);
        questRewardTMP.text = $"奖励: {quest.GoldReward}金币, " +
                              $"{quest.ExpReward}Exp, " +
                              $"x{quest.ItemReward.Quantity}{quest.ItemReward.Item.Name}";
        statusTMP.text = $"进度\n{quest.CurrentStatus}/{quest.QuestGoal}";
    }

    public void ClaimQuest()
    {
        // 完成任务领取奖励的按钮
        GameManager.instance.AddPlayerExp(QuestToComplete.ExpReward);
        Inventory.instance.AddItem(QuestToComplete.ItemReward.Item, QuestToComplete.ItemReward.Quantity);
        CoinManager.instance.AddCoins(QuestToComplete.GoldReward);
        gameObject.SetActive(false);
    }

    private void QuestCompletedCheck()
    {
        if (QuestToComplete.QuestCompleted)
        {
            claimButton.SetActive(true);
            questStatus.SetActive(false);
        }
    }

    private void OnEnable()
    {
        QuestCompletedCheck();
    }
}
