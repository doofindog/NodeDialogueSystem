using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DialogueEditor;
using TextEditor = DialogueEditor.TextEditor;


public class DialogueWindowManager : MonoBehaviour
{
    private static DialogueWindow window;

    
    public static void OpenDialogueWindow(DialoguesScriptable dialoguesScriptable)
    {
        window = (DialogueWindow) EditorWindow.GetWindow(typeof(DialogueWindow));
        window.Initialise(dialoguesScriptable);
        window.Show();
    }
    
}

public class DialogueEditorConfig
{
    public class NodeConfig
    {
        public int ID;
        public Vector2 padding;
        public Rect rect;
        public string title;
        public Vector2 spacing;
        public GUIStyle style;
        public Action<NodeEditor> OnRemoveNode;
        public bool isSelected;
        public bool canDrag;
        
        public TextEditor TextEditor;
        public List<OptionEditor> options;
        public ConnectorEditor inConnector;
        public List<ConnectorEditor> outConnector;
        

        public NodeConfig(Vector2 position, Vector2 size, Vector2 padding,Action<NodeEditor> onRemoveNode)
        {
            this.padding = padding;
            rect = new Rect(position, size);
            style = new GUIStyle();
            style.normal.background = EditorGUIUtility.Load("Assets/DialogueSystem/Sprites/PNG/red_panel.png") as Texture2D;
            style.border = new RectOffset(10, 10, 10, 10);

            OnRemoveNode = onRemoveNode;
            TextEditor = new TextEditor();
            options = new List<OptionEditor>();
            inConnector = new ConnectorEditor();

        }
        
    }
    
            
    public class TextConfig
    {
        public string text;
        public Rect rect;
        public GUIStyle style;
        
    }
    
    public class ButtonConfig
    {
        public Rect rect;
        public Action func;
        
    }

    public class OptionConfig
    {
        public Rect rect;
        public TextEditor textEditor;

        public OptionConfig()
        {
            textEditor = new TextEditor();
        }
    }
    
    public class ConnectorConfig
    {
        public Rect Rect;
    }
}
