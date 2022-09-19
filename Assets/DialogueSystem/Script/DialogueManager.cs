using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    
    [SerializeField] private ConversationGraph currentConversation;
    [SerializeField] private Entry currentEntry;
    [SerializeField] private DialogueSystemUI dialogueUI;

    
    [SerializeField] private bool inProgress;
    [SerializeField] private bool isTalking;
    [SerializeField] private bool skipTalking;
    [SerializeField] private bool finishedTalking;
    
    

    public void Awake()
    {
        DialogueEvents.startConversationEvent += StartConversation;
        DialogueUIEvents.optionPressed += HandleOnClickOption;
        DialogueUIEvents.printingCompleted += HandleTalkingComplete;
    }

    public void OnDestroy()
    {
        DialogueEvents.startConversationEvent -= StartConversation;
        DialogueUIEvents.optionPressed -= HandleOnClickOption;
        DialogueUIEvents.printingCompleted -= HandleTalkingComplete;
    }

    private void StartConversation(ConversationGraph graph)
    {
        if (graph == null) { return; }
        if (inProgress == true) { return; }

        Debug.Log("New Conversation Started");
        currentConversation = graph;
        inProgress = true;
        isTalking = true;
        SetupUI(graph);
        DialogueEvents.ConversationStarted();
    }

    private void SetupUI(ConversationGraph graph)
    {
        dialogueUI.gameObject.SetActive(true);
        currentEntry = graph.GetStart();
        dialogueUI.DisplayDialogue(currentEntry);
    }

    private void UpdateUI(Entry entry)
    {
        dialogueUI.DisplayDialogue(entry);
    }

    public void EndConversation()
    {
        inProgress = false;
        isTalking = false;
        currentEntry = null;
        dialogueUI.gameObject.SetActive(false);
        DialogueEvents.ConversationEnded();
    }

    public void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { ControlConversation();}
    }

    private void ControlConversation()
    {
        if (isTalking)
        {
            TrySkipTalking();
            return;
        }
        
        if (currentEntry.GetType() != typeof(DecisionEntry))
        {
            Entry nextEntry = currentConversation.GetNext(currentEntry);
            MoveToNext(nextEntry);
        }
    }

    private void TrySkipTalking()
    {
        if (!isTalking)
        {
            return;
        }

        DialogueEvents.skipTalking();
        isTalking = false;
    }

    private void HandleTalkingComplete()
    {
        isTalking = false;
    }

    private void MoveToNext(Entry nextEntry)
    {
        if (nextEntry == null)
        {
            EndConversation();
            return;
        }
        
        currentEntry = nextEntry;
        UpdateUI(nextEntry);
        currentEntry.Invoke();
        isTalking = true;
    }

    private void HandleOnClickOption(int index)
    {
        DecisionEntry decisionEntry = (DecisionEntry) currentEntry;
        Option optionPressed = decisionEntry.GetOptionAtIndex(index);
        
        Entry nextEntry = currentConversation.GetNext(currentEntry, optionPressed);
        MoveToNext(nextEntry);
    }
}
