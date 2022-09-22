using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class NpcDialogueTrigger : MonoBehaviour
{
    public DialogueDatabase _dialogueList;
    public ConversationGraph _selectedConversation;


    public void Awake()
    {
        Debug.Log(_selectedConversation.GetName());
    }

    public void TriggerConversation()
    {
        Debug.Log(_selectedConversation.GetName());
        FireConversation(_selectedConversation);
    }

    private void FireConversation(ConversationGraph graph)
    {
        DialogueEvents.StartConversation(graph);
    }
}

