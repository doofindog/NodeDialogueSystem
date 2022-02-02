using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Graph dialogue;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DialogueManager.instance.LoadDialogue(dialogue);
        }
    }
}
