using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{ 
    [Header("Config")]
    [SerializeField] private Quest[] quests;

    [Header("NPC Quest Panel")]
    [SerializeField] private QuestCardNPC questCardNpcPrefab;
    [SerializeField] private Transform npcPanelContainer;

    [Header("Player Quest Panel")]
    [SerializeField] private QuestCardPlayer questCardPlayerPrefab;
    [SerializeField] private Transform playerPanelContainer;
 
    private void Start()
    {
        LoadQuestsIntroNPCPanel();
    }

    public void AcceptQuest(Quest quest)
    {
        QuestCardPlayer playerCard = Instantiate(questCardPlayerPrefab, playerPanelContainer);
        playerCard.ConfigQuestUI(quest);
    }

    public void AddProgress(string questID, int amount)
    {
        Quest questToUpdate = QuestExists(questID);
        if (questToUpdate == null) return;
        if (questToUpdate.QuestAccepted)
        {
            questToUpdate.AddProgress(amount);
        }
    }

    private Quest QuestExists(string questID)
    {
        foreach (Quest quest in quests)
        {
            if (quest.ID == questID) return quest;
        }
        return null;
    }

    private void LoadQuestsIntroNPCPanel()
    {
        // 生成npc任务菜单
        for (int i = 0; i < quests.Length; i++)
        {
            QuestCard npcCard = Instantiate(questCardNpcPrefab, npcPanelContainer);
            npcCard.ConfigQuestUI(quests[i]);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            quests[i].ResetQuest();
        }
    }
}
