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
    public class DatabaseEditorManager
    {
        public static DatabaseWindow window;

        private static Dictionary<Type, Type> m_editors = new Dictionary<Type, Type>();
        
        static DatabaseEditorManager()
        {
            CacheCustomNodeEditor();
        }
        
        public static void OpenDatabaseWindow(DialogueDatabase dialogueDB)
        {
            window = (DatabaseWindow) EditorWindow.GetWindow(typeof(DatabaseWindow),false,"Dialogue Database");
            window.Initialise(dialogueDB);
            window.Show();
        }
        
        public static void SaveData()
        {
            if (window == null)
            {
                return;
            }

            if (window.dialogueDB != null)
            {
                EditorUtility.SetDirty(window.dialogueDB);
                AssetDatabase.SaveAssets();
            }
        }

        private static void CacheCustomNodeEditor()
        {
            Type[] nodeEditors = ReflectionHandler.GetDerivedTypes(typeof(Node));
            for (int i = 0; i < nodeEditors.Length; i++)
            {
                CustomNodeEditorAttribute NodeEditorAttrib = nodeEditors[i].GetCustomAttribute(typeof(CustomNodeEditorAttribute)) as CustomNodeEditorAttribute;
                if (NodeEditorAttrib != null) { m_editors.Add(NodeEditorAttrib.GetInspectedType(), nodeEditors[i]); }
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