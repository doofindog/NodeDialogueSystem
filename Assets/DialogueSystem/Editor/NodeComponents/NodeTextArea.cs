using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Editor.NodeComponents
{
    public class NodeTextArea : NodeComponent
    {
        private string _text;
        private GUIStyle _style;

        public NodeTextArea(Rect rect)
        {
            canvasRect = rect;
        }

        public string Draw(string text = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = _text;
            }

            GUI.skin.textArea.wordWrap = true;
            _text = GUI.TextArea(canvasRect, text);
            GUI.skin.textArea.wordWrap = true;
            return _text;
        }
    }
}

