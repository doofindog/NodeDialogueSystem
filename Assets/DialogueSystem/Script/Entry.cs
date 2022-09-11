using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DialogueSystem
{
    [System.Serializable]
    public abstract class Entry : ScriptableObject
    {
        public Vector2 position;
        
        public ConversationGraph CommunicationGraph;
        public string id;

        public virtual void Init(Vector2 position)
        {
            this.id = NodeManager.GenerateUniqueId();
            this.position = position;
        }

        public virtual void Invoke()
        {
            
        }
    }
}

