using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace DialogueSystem
{
    [System.Serializable]
    public class Connection
    {
        public string ID;
        public Option option;
        public DialogueSystem.Dialogue startDialogue;  
        public DialogueSystem.Dialogue endDialogue;

        public Connection(Dialogue startDialogue,Dialogue endDialogue)
        {
            ID = DialoguesManager.GenerateUniqueId();
            this.startDialogue = startDialogue; 
            this.endDialogue = endDialogue;
        }

        public Connection(Option option,Dialogue startDialogue,Dialogue endDialogue)
        {
            this.option = option;
            this.startDialogue = startDialogue;
            this.endDialogue = endDialogue;
        }
    }
}


