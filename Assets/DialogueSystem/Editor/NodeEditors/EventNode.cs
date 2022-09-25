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

        public override void Init(Entry p_entry, DatabaseWindow p_databaseWindow)
        {
            base.Init(p_entry, p_databaseWindow);
            
            _eventEntry = (EventEntry) p_entry;
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
                NodeComponentUtilt.DrawSpace(5);
                NodeComponentUtilt.DrawLine();
                NodeComponentUtilt.DrawSpace(5);
                
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////

                #region ----> Draw Types <----

                int typeIndex = -1;
                
                Type[] types = CachedData.GetEventTypes();
                string[] typeNames = new string[types.Length];
                for (int j = 0; j < types.Length; j++)
                {
                    typeNames[j] = types[j].Name;
                    
                    if(_eventEntry.staticEvents[i].eventType == null) {continue;}
                    if(string.IsNullOrEmpty(_eventEntry.staticEvents[i].eventType.typeName)) {continue;}
                    
                    string currentEventName = _eventEntry.staticEvents[i].eventType.typeName;
                    if (currentEventName == typeNames[j])
                    {
                        typeIndex = j;
                    }
                }
                
                EditorGUI.BeginChangeCheck();
                typeIndex = NodeComponentUtilt.DrawPopUp(typeIndex, typeNames, 20);
                if (EditorGUI.EndChangeCheck())
                {
                    _eventEntry.staticEvents[i].SetEventType(types[typeIndex]);
                }

                if (typeIndex == -1) { continue; }

                #endregion
                
                
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////

                #region ----> Draw Method Functions <----
                
                Type selectedType = types[typeIndex];
                
                int methodIndex = -1;
                MethodInfo[] methods = CachedData.GetMethods(selectedType);
                string[] methodNames = new string[methods.Length];
                for (int j = 0; j < methods.Length; j++)
                {
                    methodNames[j] = methods[j].Name;
                    
                    if(_eventEntry.staticEvents[i].eventMethod == null) { continue;}
                    if(string.IsNullOrEmpty(_eventEntry.staticEvents[i].eventMethod.methodName)) { continue; }

                    string currentMethodName = _eventEntry.staticEvents[i].eventMethod.methodName;
                    if (currentMethodName == methodNames[j])
                    {
                        methodIndex = j;
                    }
                }

                EditorGUI.BeginChangeCheck();
                methodIndex = NodeComponentUtilt.DrawPopUp(methodIndex, methodNames, 20);
                if (EditorGUI.EndChangeCheck())
                {
                    _eventEntry.staticEvents[i].SetMethodInfo(methods[methodIndex]);
                    SaveManager.SaveData(_eventEntry);
                }

                if (methodIndex == -1) { continue; }

                #endregion
                
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////

                #region ----> Draw parameters <----

                foreach (SerializableObjectVariable param in _eventEntry.staticEvents[i].GetParameterObj())
                {
                    DrawParameters(param);
                }

                #endregion
                
            }
        }

        private void DrawParameters(SerializableObjectVariable p_paramObj)
        {
            switch (p_paramObj.GetObjType().Name)
            {
                case "Int32":
                {
                    int value = 0;
                    
                    if (p_paramObj.GetObject() != null)
                    {
                        value = (int) p_paramObj.GetObject();
                    }
                    
                    value = Int32.Parse(NodeComponentUtilt.DrawTextField(p_paramObj.variableName, value.ToString()));
                    p_paramObj.SetObject(value);
                    
                    break;
                }
                case "Boolean":
                {
                    bool value = false;

                    if (p_paramObj.GetObjType() != null)
                    {
                        if (p_paramObj.GetObject() == null)
                        {
                            value = false;
                        }
                        else
                        {
                            value = (bool) p_paramObj.GetObject();
                        }
                    }
                    
                    value = NodeComponentUtilt.DrawToggle(p_paramObj.variableName,value);
                    p_paramObj.SetObject(value);
                    
                    break;
                }
                case "String":
                {
                    string value = string.Empty;
                    
                    if (p_paramObj.GetObject() != null)
                    {
                        if (p_paramObj.GetObject() == null)
                        {
                            value = string.Empty;
                        }
                        else
                        {
                            value = (string) p_paramObj.GetObject();
                        }
                    }
                    
                    value = NodeComponentUtilt.DrawTextField(p_paramObj.variableName, value);
                    p_paramObj.SetObject(value);
                    break;
                }
            }
            
            SaveManager.SaveData(_eventEntry);
        }

        private void AddEvent()
        {
            _eventEntry.AddNewEvent();
        }

    }
}
