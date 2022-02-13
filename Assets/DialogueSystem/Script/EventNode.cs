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
        [SerializeField] public SerializableMethodInfo m_method;
        [SerializeField] public SerializableType m_type;
        
        public void SetEvent(MethodInfo method)
        {
            m_method.methodInfo = method;
            m_type = m_method.type;
        }

        public override void Invoke()
        {
            m_method.methodInfo.Invoke(null, null);
            DialogueManager.instance.ShowDiloague(text);
        }
    }
}

