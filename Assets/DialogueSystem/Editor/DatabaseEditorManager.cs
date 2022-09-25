using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;

namespace DialogueSystem.Editor
{
    [InitializeOnLoad]
    public class DatabaseEditorManager
    {
        public static DatabaseWindow WINDOW;
        private static Dictionary<Type, Type> _editors = new Dictionary<Type, Type>();
        
        static DatabaseEditorManager()
        {
            CacheCustomNodeEditor();
        }
        
        public static void OpenDatabaseWindow(DialogueDatabase p_dialogueDB)
        {
            WINDOW = (DatabaseWindow) EditorWindow.GetWindow(typeof(DatabaseWindow),false,"Dialogue Database");
            WINDOW.Initialise(p_dialogueDB);
            WINDOW.Show();
        }
        
        public static void SaveData()
        {
            if (WINDOW == null) { return; }

            if (WINDOW.dialogueDB != null)
            {
                EditorUtility.SetDirty(WINDOW.dialogueDB);
                AssetDatabase.SaveAssets();
            }
        }

        private static void CacheCustomNodeEditor()
        {
            Type[] nodeEditors = ReflectionHandler.GetDerivedTypes(typeof(Node));
            
            for (int i = 0; i < nodeEditors.Length; i++)
            {
                CustomNodeEditorAttribute nodeEditorAttribute = nodeEditors[i].GetCustomAttribute(typeof(CustomNodeEditorAttribute)) as CustomNodeEditorAttribute;
                
                if (nodeEditorAttribute != null)
                {
                    _editors.Add(nodeEditorAttribute.GetInspectedType(), nodeEditors[i]);
                }
            }
        }
        
        public static Type GetCustomEditor(Type p_type)
        {
            if (p_type == null) { return null; }
            if (_editors == null) { CacheCustomNodeEditor(); }

            System.Type result = null;
            
            if (_editors.TryGetValue(p_type, out result))
            {
                return result;
            }
            
            return GetCustomEditor(p_type.BaseType);
        }
    }
}