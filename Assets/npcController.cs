using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class npcController : MonoBehaviour
{
    private CharacterDialogue dialogues;
    public void Awake()
    {
        dialogues = GetComponent<CharacterDialogue>(); 
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }
    }

    public void OnInteract()
    {
        DialogueManager.instance.OpenDialogue(dialogues.m_characterDialogue);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnInteract();
            }
        }
    }
}
