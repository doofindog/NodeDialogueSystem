using System;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class LinkEditor
    {
        private Link _link;
        private Action<LinkEditor> _onClickCallBack;

        public LinkEditor(Link p_link)
        {
            _link = p_link;
        }

        public void Draw(Vector2 startPos, Vector2 endPos)
        {
            Handles.DrawBezier
            (
                startPos, endPos,                                         // draw line
                startPos + Vector2.up * 50f, endPos + Vector2.down * 50f, // draw tangent
                Color.white,null, 2f                                      // colour
            );
            
            if (Handles.Button((startPos + endPos) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                DialogueEditorEvents.RemoveLink(this);
            }
        }

        public Link GetLink()
        {
            return _link;
        }
    }
} 



