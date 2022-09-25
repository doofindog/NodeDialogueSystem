using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Editor.NodeComponents
{
    public class NodeTextArea : NodeComponent
    {
        private string _text;

        public NodeTextArea(Rect rect)
        {
            base.rect = rect;
        }

        public string Draw(string text = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = _text;
            }

            GUI.skin.textArea.wordWrap = true;
            _text = GUI.TextArea(rect, text);
            GUI.skin.textArea.wordWrap = true;
            return _text;
        }
    }
}

