using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


namespace DialogueSystem
{
    [System.Serializable]
    public class Port
    {
        [SerializeField,ReadOnly] public string id;
        [SerializeField] private Node m_node;
        [SerializeField] private DirectionFlow m_DirectionFlow;


        public Port(Node node, DirectionFlow directionFlow)
        {
            id = NodeManager.GenerateUniqueId();
            m_node = node;
            m_DirectionFlow = directionFlow;
        }

        public Node GetNode()
        {
            return m_node;
        }

        public DirectionFlow GetFlow()
        {
            return m_DirectionFlow;
        }
    }

    public enum DirectionFlow
    {
        IN,
        OUT,
    }

}
