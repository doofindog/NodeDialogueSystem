using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DialogueSystem.Editor;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [InitializeOnLoad]
    public class EditorManager
    {
        public static GraphWindow window;
        private static string FOLDER_NAME = "DialogueEditorData";
        private static string PATH = "/DialogueSystem/DialogueData";

        private static Dictionary<Type, Type> m_editors = new Dictionary<Type, Type>();
        
        static EditorManager()
        {
            CacheCustomNodeEditor();
        }
        
        public static void OpenDialogueWindow(Graph graph)
        {
            window = (GraphWindow) EditorWindow.GetWindow(typeof(GraphWindow));
            window.Initialise(graph);
            window.Show();
        }
        
        public static void SaveData()
        {
            if (window.graph != null)
            {
                EditorUtility.SetDirty(window.graph);
                AssetDatabase.SaveAssets();
            }
        }

        private static void CacheCustomNodeEditor()
        {
            Type[] nodeEditors = ReflectionHandler.GetDerivedTypes(typeof(NodeEditor));
            for (int i = 0; i < nodeEditors.Length; i++)
            {
                CustomNodeEditorAttribute attrib = nodeEditors[i].GetCustomAttribute(typeof(CustomNodeEditorAttribute)) as CustomNodeEditorAttribute;
                m_editors.Add(attrib.GetInspectedType(),nodeEditors[i]);
            }
        }
        
        public static Type GetCustomEditor(Type type)
        {
            if (type == null)
            {
                return null;
            }
            if (m_editors == null)
            {
                CacheCustomNodeEditor();
            }

            System.Type result;
            if (m_editors.TryGetValue(type, out result))
            {
                return result;
            }

            return GetCustomEditor(type.BaseType);
        }
        

        public static String GetName(string fullPath)
        {
            return string.Empty;
        }
    }
}