using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    private NpcDialogueTrigger _dialogueTrigger;
    public void Start()
    {
        _dialogueTrigger = GetComponent<NpcDialogueTrigger>();
    }

    public void OnInteracted()
    {
        _dialogueTrigger.TriggerConversation();
    }
}
