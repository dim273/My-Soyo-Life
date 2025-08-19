using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Quest[] quests;

    [SerializeField] private QuestCardNPC questCardNpcPrefab;
    [SerializeField] private Transform npcPanelContainer;

    private void Start()
    {
        LoadQuestsIntroNPCPanel();
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
