using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;


public class DialoguesScriptable : ScriptableObject
{
    public string id;
    public List<Dialogue> dialogues; //TODO : Could be changed to a HashTable


    public DialoguesScriptable()
    {
        Initalise();
    }

    private void Initalise()
    {
        id = DialoguesManager.GenerateUniqueId();
        dialogues = new List<Dialogue>();
    }

    public Dialogue CreateDialogue()
    {
        Dialogue dialogue = new Dialogue();
        dialogues.Add(dialogue);
        return dialogue;
    }

    public void RemoveDialogue(Dialogue dialogue)
    {
        dialogues.Remove(dialogue);
    }

}
