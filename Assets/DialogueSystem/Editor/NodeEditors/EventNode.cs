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

        public override void Init(Entry entry, DatabaseWindow databaseWindow)
        {
            base.Init(entry, databaseWindow);
            _eventEntry = (EventEntry) entry;
        }

        protected override void ConfigMenu()
        {
            base.ConfigMenu();
            menu.AddItem(new GUIContent("Add Function"), false, AddEvent );
        }
        
        protected override void DrawComponents()
        {
            inPort = NodeComponentUtilt.DrawPort(PortType.In, HandleInPortSelect);
            
            _eventEntry.text = NodeComponentUtilt.DrawText(_eventEntry.text, 50);
            
            NodeComponentUtilt.DrawSpace(10);
            
            DrawMethodList();
            
            outPort = NodeComponentUtilt.DrawPort(PortType.Out, HandleOutPortSelect);
        }

        private void DrawMethodList()
        {
            if (_eventEntry.staticEvents == null)
            {
                return;
            }

            for(int i = 0; i < _eventEntry.staticEvents.Count;i++)
            {
                NodeComponentUtilt.DrawLine();
                NodeComponentUtilt.DrawSpace(5);
                
                int typeIndex = 0;
                string currentEventName = _eventEntry.staticEvents[i].eventType.typeName;

                Type[] types = CachedData.GetEventTypes();
                string[] typeNames = new string[types.Length];
                for (int j = 0; j < types.Length; j++)
                {
                    typeNames[j] = types[j].Name;
                    if (currentEventName == typeNames[j])
                    {
                        typeIndex = j;
                    }
                }

                typeIndex = NodeComponentUtilt.DrawPopUp(typeIndex, typeNames, 20);
                Type selectedType = types[typeIndex];
                _eventEntry.staticEvents[i].SetEventType(selectedType);

                int methodIndex = 0;
                string currentMethodName = _eventEntry.staticEvents[i].eventMethod.methodName;

                MethodInfo[] methods = CachedData.GetMethods(selectedType);
                string[] methodNames = new string[methods.Length];
                for (int j = 0; j < methods.Length; j++)
                {
                    methodNames[j] = methods[j].Name;
                    if (currentMethodName == methodNames[j])
                    {
                        methodIndex = j;
                    }
                }

                methodIndex = NodeComponentUtilt.DrawPopUp(methodIndex, methodNames, 20);
                MethodInfo selectedMethod = methods[methodIndex];
                _eventEntry.staticEvents[i].SetMethodInfo(selectedMethod);
                
                NodeComponentUtilt.DrawSpace(5);
            }
        }

        private void AddEvent()
        {
            _eventEntry.AddNewEvent();
        }

    }
}
