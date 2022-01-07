using System;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class OptionEditor
    {
        public Rect rect;
        public Option option;
        private Action<OptionEditor> removeCallback;


        public OptionEditor(Option option, Action<OptionEditor> removeCallback)
        {
            this.option = option;
            this.removeCallback = removeCallback;
        }

        public void Draw(Rect rect)
        {
            this.rect = rect;
            option.text = GUI.TextArea(rect, option.text);

            Vector2 size = new Vector2(10, 10);
            Vector2 positon = rect.position;
            positon.x += rect.size.x  - (size.x * 0.5f);
            positon.y -= size.y * 0.5f;

            Rect buttonrect = new Rect(positon, size);

            if (GUI.Button(buttonrect, String.Empty))
            {
                removeCallback(this);
            }
        }

        public string GetText()
        {
            return option.text;
        }

        public string GetOptionID()
        {
            return option.id;
        }
    }
}

