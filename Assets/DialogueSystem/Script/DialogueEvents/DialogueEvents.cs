using System;

using UnityEngine;

using DialogueSystem;

[System.Serializable]
public abstract class DialogueEvents : MonoBehaviour
{
    public static Action<ConversationGraph> startConversationEvent;
    public static void StartConversation(ConversationGraph p_graph)
    {
        if (p_graph == null)
        {
            return;
        }

        startConversationEvent?.Invoke(p_graph);
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
    public static void OptionPressed(int p_index)
    {
        optionPressed?.Invoke(p_index);
    }
}
