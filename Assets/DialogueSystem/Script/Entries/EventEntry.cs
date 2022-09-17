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
        
        [SerializeField] public List<EventInfo> staticEvents;
        [SerializeField] private UnityEvent unityEvent;

        public override void Init(Vector2 position, ConversationGraph graph)
        {
            base.Init(position, graph);
            staticEvents = new List<EventInfo>();
            
            AddNewEvent();
        }

        public void AddNewEvent()
        {
            EventInfo eventInfo = new EventInfo();
            staticEvents.Add(eventInfo);
        }

        public override void Invoke()
        {
            //m_method.methodInfo.Invoke(null, null);
        }

        public void Invoke(object[] parameters)
        {
            //m_method.methodInfo.Invoke(null, parameters);
        }
    }
}

