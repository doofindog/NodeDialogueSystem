using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    [SerializeField] private ConversationGraph mConversation;
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private Text m_text;
    [SerializeField] private float m_textSpeed;
    [SerializeField] private KeyCode m_key;
    private bool fastShow;
    private bool endReached;
    
    private ConversationGraph _mConversationGraph;
    private Entry currentIndex;

    private IEnumerator _enumerator;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.gameObject.SetActive(false);
        }
    }

    public void OpenDialogue(ConversationGraph conversationGraph)
    {
        mConversation = conversationGraph;
        LoadDialogue(mConversation);
        
    }

    public void ShowDiloague(string text, List<Option> option = null)
    {
        dialogueUI.SetActive(true);
        if (_enumerator != null)
        {
            StopCoroutine(_enumerator);
        }

        _enumerator = ShowText(text, option);
        StartCoroutine(_enumerator);
    }
    
    private IEnumerator ShowText(string text, List<Option> options = null)
    {
        if (text != string.Empty)
        {
            m_text.text = string.Empty;
            if (fastShow != true)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    m_text.text += text[i];
                    yield return new WaitForSeconds(m_textSpeed);
                }
            }
            else
            {
                m_text.text = text;
            }

            if (options != null)
            {

            }
        }
    }
    public void CloseDialogue()
    {
        dialogueUI.SetActive(false);
        this.gameObject.SetActive(false);
    }
    
    public void LoadDialogue(ConversationGraph conversationConversationGraph)
    {
        instance.gameObject.SetActive(true);
        dialogueUI.gameObject.SetActive(true);
        _mConversationGraph = conversationConversationGraph;
    }
    
}
