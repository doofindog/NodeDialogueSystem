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
        public DialogueSystem.Entry startEntry;  
        public DialogueSystem.Entry endEntry;

        public Connection(Entry startEntry,Entry endEntry)
        {
            ID = NodeManager.GenerateUniqueId();
            this.startEntry = startEntry; 
            this.endEntry = endEntry;
        }

        public Connection(Option option,Entry startEntry,Entry endEntry)
        {
            this.option = option;
            this.startEntry = startEntry;
            this.endEntry = endEntry;
        }
    }
}


