using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DialogueSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;


namespace DialogueSystem
{
    [System.Serializable]
    public class EventNode : Node
    {
        [SerializeField] public string text;
        [SerializeField] public DialogueEvents m_eventObj;
        [SerializeField] public MethodInfo m_method;

        public void SetEventObj(Type eventType)
        {
            m_eventObj = Activator.CreateInstance(eventType) as DialogueEvents;
        }
        
       

    }
}

