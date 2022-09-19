using System;
using DialogueSystem.Editor.NodeComponents;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class LinkEditor
    {
        private Link link;
        private Action<LinkEditor> _onClickCallBack;

        public LinkEditor(Link p_link)
        {
            link = p_link;
        }

        public void Draw(Vector2 startPos, Vector2 endPos)
        {

            Handles.DrawBezier
            (
                startPos, endPos,
                startPos + Vector2.up * 50f , endPos + Vector2.down * 50f, 
                Color.white,null, 2f
            );
            
            if (Handles.Button((startPos + endPos) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                DialogueEditorEvents.RemoveLink(this);
            }
        }

        public Link GetLink()
        {
            return link;
        }
    }
} 



