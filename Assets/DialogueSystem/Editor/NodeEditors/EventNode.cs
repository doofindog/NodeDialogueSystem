using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomNodeEditor(typeof(DialogueSystem.EventEntry))]
    public class EventNode : Node
    {
        private EventEntry _eventEntry;
        
        private string[] _methodNames;
        private string[] _typeNames;
        
        private int typeIndex;
        private int methodIndex;

        private List<Rect> _eventRect;
        
        public override void Init(Entry entry, DatabaseWindow databaseWindow)
        {
            base.Init(entry, databaseWindow);
            _eventEntry = (EventEntry) entry;
            _eventRect = new List<Rect>();
        }

        protected override void DrawComponents()
        {
            _eventRect.Clear();
            inPort = NodeComponentUtilt.DrawPort(PortType.In, HandleInPortSelect);
            
            _eventEntry.text = NodeComponentUtilt.DrawText(_eventEntry.text, 50);
            
            NodeComponentUtilt.DrawSpace(10);

            DrawMethodList();
            
            outPort = NodeComponentUtilt.DrawPort(PortType.Out, HandleOutPortSelect);
        }

        public void DrawMethodList()
        {
            System.Type[] eventTypes = CachedData.eventTypes;
            string[] eventNames = new string[eventTypes.Length];
            for (int i = 0; i < eventTypes.Length; i++)
            {
                eventNames[i] = eventTypes[i].Name;
                if (_eventEntry.m_type.type != null)
                {
                    if (eventNames[i].Equals(_eventEntry.m_type.type.Name))
                    {
                        typeIndex = i;
                    }
                }
            }
            
            typeIndex = NodeComponentUtilt.DrawPopUp(typeIndex, eventNames, 20);
            
            MethodInfo[] methods = CachedData.GetMethods(eventTypes[typeIndex]);
            string[] methodNames = new string[methods.Length];
            for (int i = 0; i < methods.Length; i++)
            {
                methodNames[i] = methods[i].Name;
                if (_eventEntry.m_method.methodInfo != null)
                {
                    if (_eventEntry.m_method.methodInfo.Name == methodNames[i])
                    {
                        methodIndex = i;
                    }
                }
            }
            
            methodIndex = NodeComponentUtilt.DrawPopUp(methodIndex, methodNames, 20);
            _eventEntry.SetEvent(methods[methodIndex]);
        }

    }
}
