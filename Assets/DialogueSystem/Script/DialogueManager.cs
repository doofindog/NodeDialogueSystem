using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private Text m_text;
    [SerializeField] private float m_textSpeed;
    [SerializeField] private KeyCode m_key;
    private bool fastShow;
    private bool textCompleted;
    private bool endReached;
    
    private Graph m_graph;
    private Node currentIndex;

    private IEnumerator _enumerator;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.gameObject.SetActive(false);
        }
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

            textCompleted = true;
        }
    }
    public void CloseDialogue()
    {
        dialogueUI.SetActive(false);
        this.gameObject.SetActive(false);
    }
    
    public void LoadDialogue(Graph dialogueGraph)
    {
        instance.gameObject.SetActive(true);
        dialogueUI.gameObject.SetActive(true);
        m_graph = dialogueGraph;
        currentIndex = m_graph.GetStarNode();
        MoveToNextNode();
    }
    
    public void MoveToNextNode()
    {
        Node node = m_graph.GetNext(currentIndex);
        if (node != null)
        {
            currentIndex = node;
            if (node.GetType() == typeof(PointNode))
            {
                CloseDialogue();
            }
            else
            {
                currentIndex.Invoke();
            }
        }


    }

    public void OnClick()
    { 
        currentIndex.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_key))
        {
            Debug.Log("Called");
            if (textCompleted != true)
            {
                fastShow = true;
            }
            else
            {
                MoveToNextNode();
            }
        }
    }
}
