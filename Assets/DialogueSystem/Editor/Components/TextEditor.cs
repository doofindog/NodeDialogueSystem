using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace  DialogueEditor
{
    public class TextEditor
    { 
        public DialogueEditorConfig.TextConfig config;

        public TextEditor()
        {
            config = new DialogueEditorConfig.TextConfig();
        }
        

        public void Draw(Rect rect)
        {
            config.rect = rect;
            config.text = GUI.TextArea(rect, config.text);
        }

        public string GetText()
        {
            return config.text;
        }
    }
}

