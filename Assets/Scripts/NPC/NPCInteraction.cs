using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private NPCDialogue dialogueToShow;
    [SerializeField] private GameObject interactionBox;

    public NPCDialogue DialogueToShow => dialogueToShow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.instance.NPCSelected = this;
            interactionBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.instance.NPCSelected = null;
            DialogueManager.instance.CloseDialoguePanel();
            interactionBox.SetActive(false);
        }
    }
}
