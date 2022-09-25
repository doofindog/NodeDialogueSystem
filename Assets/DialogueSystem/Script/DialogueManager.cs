using DialogueSystem;

using UnityEngine;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    
    
    [SerializeField] private Entry _currentEntry;
    [SerializeField] private DialogueSystemUI _dialogueUI;
    [SerializeField] private ConversationGraph _currentConversation;
    [SerializeField] private bool _inProgress;
    [SerializeField] private bool _isTalking;
    [SerializeField] private bool _skipTalking;
    [SerializeField] private bool _finishedTalking;
    [SerializeField] private bool _freezeInputs;
    
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

    private void StartConversation(ConversationGraph p_graph)
    {
        if (p_graph == null) { return; }
        if (_inProgress == true) { return; }
        
        _currentConversation = p_graph;
        _currentEntry = _currentConversation.GetNext(p_graph.GetStart());

        _freezeInputs = false;
        _inProgress = true;
        _isTalking = true;
        
        SetupUI(p_graph);
        
        DialogueEvents.ConversationStarted();
    }

    private void SetupUI(ConversationGraph graph)
    {
        _dialogueUI.gameObject.SetActive(true);
        _dialogueUI.DisplayDialogue(_currentEntry);
    }

    private void UpdateUI(Entry p_entry)
    {
        _dialogueUI.DisplayDialogue(p_entry);
    }

    private void EndConversation()
    {
        _freezeInputs = true;
        _inProgress = false;
        _isTalking = false;
        _currentEntry = null;
        _dialogueUI.gameObject.SetActive(false);
        
        DialogueEvents.ConversationEnded();
    }

    public void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        if (_freezeInputs == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) { ControlConversation();}
    }

    private void ControlConversation()
    {
        if (_isTalking)
        {
            TrySkipTalking();
            return;
        }
        
        if (_currentEntry.GetType() != typeof(DecisionEntry))
        {
            Entry nextEntry = _currentConversation.GetNext(_currentEntry);
            MoveToNext(nextEntry);
        }
    }

    private void TrySkipTalking()
    {
        if (!_isTalking)
        {
            return;
        }

        DialogueEvents.skipTalking();
        
        _isTalking = false;
    }

    private void HandleTalkingComplete()
    {
        _isTalking = false;
    }

    private void MoveToNext(Entry p_nextEntry)
    {
        if (p_nextEntry == null)
        {
            EndConversation();
            return;
        }
        
        UpdateUI(p_nextEntry);
        
        _currentEntry = p_nextEntry;
        _currentEntry.Invoke();
        _isTalking = true;
    }

    private void HandleOnClickOption(int p_index)
    {
        DecisionEntry decisionEntry = (DecisionEntry) _currentEntry;
        Option optionPressed = decisionEntry.GetOptionAtIndex(p_index);
        Entry nextEntry = _currentConversation.GetNext(_currentEntry, optionPressed);
        
        MoveToNext(nextEntry);
    }
}
