using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace  DialogueEditor
{

    public class TextEditor
    { 
        public string text;
        public Rect rect;
        public GUIStyle style;

        public TextEditor()
        {
            
        }

        public void Draw(Rect rect)
        {
            rect = rect;
            text = GUI.TextArea(rect, text);
        }

        public string GetText()
        {
            return text;
        }
    }
}

