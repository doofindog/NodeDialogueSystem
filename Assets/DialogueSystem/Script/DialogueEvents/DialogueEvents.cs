using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

[System.Serializable]
public abstract class DialogueEvents : MonoBehaviour
{
    public static Action<ConversationGraph> startConversationEvent;
    public static void StartConversation(ConversationGraph graph)
    {
        if (graph == null)
        {
            return;
        }

        startConversationEvent?.Invoke(graph);
    }
    
    public static Action conversationStartedEvent;
    public static void ConversationStarted()
    {
        conversationStartedEvent?.Invoke();
    }

    public static Action endConversation;
    public static void ConversationEnded()
    {
        endConversation?.Invoke();
    }

    public static Action skipTalking;
    public static void SkipConversation()
    {
        skipTalking?.Invoke();
    }
    
}

public abstract class DialogueUIEvents
{
    public static Action printingCompleted;

    public static void SendPrintingCompleted()
    {
        printingCompleted?.Invoke();
    }

    public static Action<int> optionPressed;

    public static void OptionPressed(int index)
    {
        optionPressed?.Invoke(index);
    }
}
