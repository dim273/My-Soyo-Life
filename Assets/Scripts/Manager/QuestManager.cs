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

    private void LoadQuestsIntroNPCPanel()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            QuestCard npcCard = Instantiate(questCardNpcPrefab, npcPanelContainer);
            npcCard.ConfigQuestUI(quests[i]);
        }
    }
}
