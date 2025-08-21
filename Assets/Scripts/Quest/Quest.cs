using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Quest : ScriptableObject
{
    [Header("Info")]
    public string Name;
    public string ID;
    public int QuestGoal;

    [Header("Description")]
    [TextArea] public string Description;

    [Header("Reward")]
    public int GoldReward;
    public float ExpReward;
    public QuestItemReward ItemReward;

    [HideInInspector] public int CurrentStatus;
    [HideInInspector] public bool QuestCompleted;
    [HideInInspector] public bool QuestAccepted;

    public void AddProgress(int amount)
    {
        // 更新任务进度
        CurrentStatus += amount;
        if (CurrentStatus >= QuestGoal)
        {
            CurrentStatus = QuestGoal;
            QuestIsCompleted();
        }
    }

    private void QuestIsCompleted()
    {
        // 任务完成
        if (QuestCompleted) return;
        QuestCompleted = true;
    }

    public void ResetQuest()
    {
        // 重置任务的状态
        QuestAccepted = false;
        QuestCompleted = false;
        CurrentStatus = 0;
    }
}

[Serializable]
public class QuestItemReward
{
    public InventoryItem Item;
    public int Quantity;
}