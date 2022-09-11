using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace DialogueSystem
{
    [System.Serializable]
    public class Link
    {
        public string id;
        
        public DialogueSystem.Entry sourceEntry;  
        public DialogueSystem.Entry destinationEntry;

        public Link(Entry sourceEntry,Entry destinationEntry)
        {
            id = NodeManager.GenerateUniqueId();
            
            this.sourceEntry = sourceEntry; 
            this.destinationEntry = destinationEntry;
        }

        public Link(Option option,Entry sourceEntry,Entry destinationEntry)
        {
            this.sourceEntry = sourceEntry;
            this.destinationEntry = destinationEntry;
        }
    }
}


