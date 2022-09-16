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
    public class EventEntry : Entry
    {
        [SerializeField, HideInInspector] public string text;
        [SerializeField, HideInInspector] public SerializableMethodInfo m_method;
        [SerializeField, HideInInspector] public SerializableType m_type;

        [SerializeField] public MethodDictionary _methods;
        
        [SerializeField] private UnityEvent listeners;

        public override void Init(Vector2 position, ConversationGraph graph)
        {
            base.Init(position, graph);
            _methods = new MethodDictionary();
            
            
        }

        public void SetEvent(MethodInfo method)
        {
            m_method.methodInfo = method;
            m_type = m_method.type;
        }

        public void AddEvent()
        {
            SerializableType type = new SerializableType(CachedData.GetEventTypes()[0]);
            SerializableMethodInfo methodInfo = new SerializableMethodInfo(CachedData.GetMethods(type.type)[0]);
            List<SerializableMethodInfo> methodInfos = new List<SerializableMethodInfo>();

            _methods ??= new MethodDictionary();
            _methods.Add(type,methodInfos);
        }
        

        public override void Invoke()
        {
            m_method.methodInfo.Invoke(null, null);
        }

        public void Invoke(object[] parameters)
        {
            m_method.methodInfo.Invoke(null, parameters);
        }
    }
}

