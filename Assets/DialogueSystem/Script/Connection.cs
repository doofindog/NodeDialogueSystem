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
        public DialogueSystem.Node startNode;  
        public DialogueSystem.Node endNode;

        public Connection(Node startNode,Node endNode)
        {
            ID = NodeManager.GenerateUniqueId();
            this.startNode = startNode; 
            this.endNode = endNode;
        }

        public Connection(Option option,Node startNode,Node endNode)
        {
            this.option = option;
            this.startNode = startNode;
            this.endNode = endNode;
        }
    }
}


