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
        public Port inPort;
        public Port outPort;

        public virtual void Init(Vector2 position)
        {
            this.id = NodeManager.GenerateUniqueId();
            this.position = position;
            inPort = new Port(this, DirectionFlow.IN);
            outPort = new Port(this, DirectionFlow.OUT);
        }

        public virtual void Invoke()
        {
            
        }

        public virtual Entry GetNext()
        {
            return outPort.GetNode();
        }
    }
}

