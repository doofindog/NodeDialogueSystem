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
        [SerializeField] private Entry mEntry;
        [SerializeField] private DirectionFlow m_DirectionFlow;


        public Port(Entry entry, DirectionFlow directionFlow)
        {
            id = NodeManager.GenerateUniqueId();
            mEntry = entry;
            m_DirectionFlow = directionFlow;
        }

        public Entry GetNode()
        {
            return mEntry;
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
