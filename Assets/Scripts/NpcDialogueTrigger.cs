using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class NpcDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueDatabase dialogueList;



    public void TriggerConversation()
    {
        FireConversation(dialogueList.GetConversationGraphAtIndex(0));
    }

    private void FireConversation(ConversationGraph graph)
    {
        DialogueEvents.StartConversation(graph);
    }
}
